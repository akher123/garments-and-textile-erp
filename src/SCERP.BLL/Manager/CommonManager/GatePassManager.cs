using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.DAL.IRepository;
using SCERP.DAL.IRepository.ICommonRepository;
using SCERP.Model.CommonModel;

namespace SCERP.BLL.Manager.CommonManager
{
    public class GatePassManager : IGatePassManager
    {
        private readonly IGatePassRepository _gatePassRepository;
        private readonly IRepository<GatePassDetail> _gatepassDetailRepository;
        public GatePassManager(IGatePassRepository gatePassRepository, IRepository<GatePassDetail> gatepassDetailRepository)
        {
            _gatePassRepository = gatePassRepository;
            _gatepassDetailRepository = gatepassDetailRepository;
        }

        public List<GatePass> GetGatePassByPaging(string typeId, string searchString, string compId, int pageIndex, string sort, string sortdir,
            out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            
            var gatepassList = _gatePassRepository.Filter(x => x.CompId == compId &&x.TypeId==typeId&& (x.RefId.Contains(searchString) || String.IsNullOrEmpty(searchString))&&
            (x.BillNo.Contains(searchString) || String.IsNullOrEmpty(searchString)));
            totalRecords = gatepassList.Count();
            switch (sort)
            {
                case "challanNo":
                    switch (sortdir)
                    {
                        case "DESC":
                            gatepassList = gatepassList
                                .OrderByDescending(r => r.ChallanNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            gatepassList = gatepassList
                                .OrderBy(r => r.ChallanNo)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    gatepassList = gatepassList
                        .OrderByDescending(r => r.GatePassId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return gatepassList.ToList();
        }

        public GatePass GetGatePassById(int gatePassId)
        {
            return _gatePassRepository.FindOne(x => x.GatePassId == gatePassId);
        }

        public List<GatePassDetail> GetGatepassDetailById(int gatePassId)
        {
            return _gatepassDetailRepository.Filter(x => x.GatePassId == gatePassId).ToList();
        }

        public string GetGatePassRefId(string compId,string typeId)
        {
            var gatePassRefId = _gatePassRepository.Filter(x => x.CompId == compId && x.TypeId == typeId).Max(x => x.RefId.Substring(3)) ?? "0";
            if (GatepassType.KnitFabType == typeId)
            {
                return "KFD" + gatePassRefId.IncrementOne().PadZero(7);
            }
            else if(GatepassType.SampleGatePass== typeId)
            {
                return "PSG" + gatePassRefId.IncrementOne().PadZero(7);
            }
            else if (GatepassType.YarnType == typeId)
            {
                return "PFL" + gatePassRefId.IncrementOne().PadZero(7);
            }
            else
            {
                return "GYR" + gatePassRefId.IncrementOne().PadZero(7);
            }
            
           
        }

        public int EditGatePass(GatePass model)
        {
            int edited = 0;
            using (var transaction = new TransactionScope())
            {

                GatePass gatePass = _gatePassRepository.FindOne(x => x.CompId == model.CompId && x.GatePassId == model.GatePassId);
                gatePass.BillNo = model.BillNo;
              
                gatePass.ChallanNo = model.ChallanNo;
                gatePass.ChallanDate = model.ChallanDate;
                gatePass.PartyName = model.PartyName;
                gatePass.Address = model.Address;
                gatePass.Through = model.Through;
                gatePass.Designation = model.Designation;
                gatePass.Remarks = model.Remarks;
                gatePass.EditedBy = PortalContext.CurrentUser.UserId;
                gatePass.EditedDate =DateTime.Now;
                gatePass.Remarks = model.Remarks;
                gatePass.BuyerName = model.BuyerName;
                gatePass.OrderName = model.OrderName;
                gatePass.StyleName = model.StyleName;
            
                edited = _gatePassRepository.Edit(gatePass);
                var detail = model.GatePassDetail.Select(x =>
                {
                    x.GatePassId = gatePass.GatePassId;
                    return x;
                });
                _gatepassDetailRepository.Delete(x => x.GatePassId == model.GatePassId);
                edited += _gatepassDetailRepository.SaveList(detail.ToList());

                transaction.Complete();
            }
            return edited;
        }

        public int DeleteGatePass(int gatePassId)
        {
            int deleted = 0;
            using (var transaction = new TransactionScope())
            {
                deleted += _gatepassDetailRepository.Delete(x => x.GatePassId == gatePassId);
                deleted += _gatePassRepository.Delete(x => x.GatePassId == gatePassId);
                transaction.Complete();
            }
            return deleted;
        }

        public int SaveGatePass(GatePass gatePass)
        {
              gatePass.CreatedBy = PortalContext.CurrentUser.UserId;
             gatePass.CreatedDate=DateTime.Now;
             gatePass.IsApproved = false;

            return _gatePassRepository.Save(gatePass);
        }

        public DataTable GatePassReport(int gatePassId)
        {
            string spQuery = String.Format("exec SpGatePass '{0}'",gatePassId);
            return _gatePassRepository.ExecuteQuery(spQuery);
        }

        public int ApprovedGatePass(int gatePassId)
        {
            GatePass gatePass = _gatePassRepository.FindOne(x => x.GatePassId == gatePassId);
            if (gatePass.IsApproved)
            {
                gatePass.ApprovedBy = null;
                gatePass.IsApproved = false;
            }
            else
            {
                gatePass.ApprovedBy = PortalContext.CurrentUser.UserId;
                gatePass.IsApproved = true;
            }
            return _gatePassRepository.Edit(gatePass);
        }

        public List<GatePass> GetGateSamplePass(string typeId, string searchString, string compId)
        {
            var pageSize = AppConfig.PageSize;
            var gatepassList = _gatePassRepository.Filter(x => x.CompId == compId && x.TypeId == typeId && x.BillNo== searchString);
                  return gatepassList.ToList();
        }
    }
}
