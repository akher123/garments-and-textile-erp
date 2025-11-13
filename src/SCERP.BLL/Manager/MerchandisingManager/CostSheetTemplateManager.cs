using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
   public class CostSheetTemplateManager: ICostSheetTemplateManager
   {
       private readonly ICostSheetTemplateRepository _costSheetTemplateRepository;

       public CostSheetTemplateManager(ICostSheetTemplateRepository costSheetTemplateRepository)
       {
           _costSheetTemplateRepository = costSheetTemplateRepository;
       }

       public List<OM_CostSheetTemplate> GetCostSheetTemplateByPaging(string particular, int pageIndex, string sort, string sortdir, out int totalRecords)
       {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var costSheetTemplate = _costSheetTemplateRepository.Filter(x => x.Particular.Trim().Contains(particular) || String.IsNullOrEmpty(particular)).Include(x=>x.OM_ItemType).Include(x=>x.OM_TempGroup);
            totalRecords = costSheetTemplate.Count();
            switch (sort)
            {
                case "Particular":
                    switch (sortdir)
                    {
                        case "DESC":
                            costSheetTemplate = costSheetTemplate
                                 .OrderByDescending(r => r.Particular)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            costSheetTemplate = costSheetTemplate
                                 .OrderBy(r => r.TemplateId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    costSheetTemplate = costSheetTemplate
                        .OrderByDescending(r => r.TemplateId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return costSheetTemplate.ToList();
        }

       public OM_CostSheetTemplate GeTcostSheetTemplateByTemplateId(int templateId, string compId)
       {
            return _costSheetTemplateRepository.FindOne(x => x.CompId == compId && x.TemplateId== templateId);
        }

       public bool IsCostSheetTemplateExist(OM_CostSheetTemplate model)
       {
            return _costSheetTemplateRepository.Exists(x => x.CompId == PortalContext.CurrentUser.CompId && x.TemplateId != model.TemplateId && x.Particular == model.Particular);
        }

       public int EditCostSheetTemplate(OM_CostSheetTemplate model)
       {
            OM_CostSheetTemplate costSheetTemplate =
                 _costSheetTemplateRepository.FindOne(
                     x => x.CompId == PortalContext.CurrentUser.CompId && x.TemplateId == model.TemplateId);
           costSheetTemplate.SerialNo = model.SerialNo;
           costSheetTemplate.Particular = model.Particular;
           costSheetTemplate.ItemTypeId = model.ItemTypeId;
           costSheetTemplate.TempGroupId = model.TempGroupId;
            return _costSheetTemplateRepository.Edit(costSheetTemplate);
        }

       public int SaveCostSheetTemplate(OM_CostSheetTemplate model)
       {
            model.CompId = PortalContext.CurrentUser.CompId;
            return _costSheetTemplateRepository.Save(model);
        }

       public int DeleteCostSheetTemplate(int templateId, string compId)
       {
            return _costSheetTemplateRepository.Delete(x => x.CompId == compId && x.TemplateId == templateId);
        }

       public List<OM_CostSheetTemplate> GetCostSheetTemplateByItemTypeId(int itemTypeId, string compId)
       {
            return _costSheetTemplateRepository.Filter(x => x.CompId == compId && x.ItemTypeId == itemTypeId).Include(x=>x.OM_TempGroup).OrderBy(x=>x.SerialNo).ToList();
        }
   }
}
