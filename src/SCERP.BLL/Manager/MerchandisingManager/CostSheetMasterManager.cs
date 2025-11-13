using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Transactions;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
   public class CostSheetMasterManager: ICostSheetMasterManager
   {
       private readonly ICostSheetMasterRepository _costSheetMasterRepository;
       private readonly ICostSheetDetailRepository _costSheetDetailRepository;

       public CostSheetMasterManager(ICostSheetMasterRepository costSheetMasterRepository, ICostSheetDetailRepository costSheetDetailRepository)
       {
           _costSheetMasterRepository = costSheetMasterRepository;
           _costSheetDetailRepository = costSheetDetailRepository;
       }

       public int SaveCostSheetMaster(OM_CostSheetMaster model)
       {
           model.CompId = PortalContext.CurrentUser.CompId;
           model.CostSheetMasterRefId = GetCostSheetMasterRefId();
           return _costSheetMasterRepository.Save(model);
       }

       public int EditCostSheetMaster(OM_CostSheetMaster model)
       {
            int edited = 0;
            using (var transaction = new TransactionScope())
            {
                _costSheetDetailRepository.Delete(x => x.CostSheetMasterId == model.CostSheetMasterId);
                var costSheetMaster = _costSheetMasterRepository.FindOne(x => x.CostSheetMasterId == model.CostSheetMasterId);
                costSheetMaster.BuyerId = model.BuyerId;
                costSheetMaster.OrderNo = model.OrderNo;
                costSheetMaster.StyleNo = model.StyleNo;
                costSheetMaster.OrderQty = model.OrderQty;
                costSheetMaster.Item = model.Item;
                costSheetMaster.Fabrication = model.Fabrication;
                costSheetMaster.Color = model.Color;
                costSheetMaster.Size = model.Size;
                costSheetMaster.YarnCount = model.YarnCount;
                costSheetMaster.Gsm = model.Gsm;
                costSheetMaster.ItemTypeId = model.ItemTypeId;
                edited = _costSheetMasterRepository.Edit(costSheetMaster);
                var detail = model.OM_CostSheetDetail.Select(x =>
                {
                    x.CostSheetMasterId = costSheetMaster.CostSheetMasterId;
                    return x;
                });
                edited += _costSheetDetailRepository.SaveList(detail.ToList());
                transaction.Complete();
            }
            return edited;
        }

       public int DeleteCostSheetMaster(long costSheetMasterId)
       {
           throw new NotImplementedException();
       }

       public List<OM_CostSheetMaster> GetCostSheetMasterByPaging(long buyerId, string orderNo, string color, int pageIndex, string sort, string sortdir,out int totalRecords)
       {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var costSheetMasterList = _costSheetMasterRepository.Filter(x => x.BuyerId==buyerId || buyerId==0);
            totalRecords = costSheetMasterList.Count();
            switch (sort)
            {
                case "OrderNo":
                    switch (sortdir)
                    {
                        case "DESC":
                            costSheetMasterList = costSheetMasterList
                                 .OrderByDescending(r => r.CostSheetMasterId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            costSheetMasterList = costSheetMasterList
                                 .OrderBy(r => r.CostSheetMasterId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    costSheetMasterList = costSheetMasterList
                        .OrderByDescending(r => r.CostSheetMasterId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return costSheetMasterList.ToList();
        }

       public string GetCostSheetMasterRefId()
       {
            var maxCostSheetMasterRefId = _costSheetMasterRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId).Max(x => x.CostSheetMasterRefId.Substring(2)) ?? "0";
            return "CS" + maxCostSheetMasterRefId.IncrementOne().PadZero(5);
        }

       public OM_CostSheetMaster GetCostSheetMasterById(long costSheetMasterId, string compId)
       {
           return _costSheetMasterRepository.GetWithInclude(x => x.CompId == compId , "OM_Buyer", "OM_ItemType").FirstOrDefault(x => x.CostSheetMasterId == costSheetMasterId); 
       }

       public List<OM_CostSheetTemplate> GetCostSheetTemplateDetailByCostSheetMasterId(long costSheetMasterId, string compId)
       {
           var costSheetTemplateList = _costSheetDetailRepository.Filter(
               x => x.CompId == compId && x.CostSheetMasterId == costSheetMasterId)
               .Include(x => x.OM_CostSheetTemplate.OM_TempGroup).ToList()
               .Select(x =>
               {
                   x.OM_CostSheetTemplate = x.OM_CostSheetTemplate;
                   x.OM_CostSheetTemplate.ParticularRate = x.ParticularRate;
                   return x.OM_CostSheetTemplate;
               }).ToList();
           return costSheetTemplateList;
       }
   }
}
