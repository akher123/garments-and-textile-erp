using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using SCERP.BLL.IManager.IMaintenance;
using SCERP.Common;
using SCERP.DAL.IRepository.IMaintenance;
using SCERP.Model.Maintenance;

namespace SCERP.BLL.Manager.Maintenance
{
    public class ReturnableChallanManager : IReturnableChallanManager
    {
        private readonly IReturnableChallanRepository _returnableChallanRepository;
        private readonly IReturnableChallanDetailRepository _returnableChallanDetailRepository;

        public ReturnableChallanManager(IReturnableChallanRepository returnableChallanRepository, IReturnableChallanDetailRepository returnableChallanDetailRepository)
        {
            _returnableChallanRepository = returnableChallanRepository;
            _returnableChallanDetailRepository = returnableChallanDetailRepository;
        }

        public List<Maintenance_ReturnableChallan> GetAllReturnableChallanByPaging(int pageIndex, string sort, string sortdir, DateTime? dateFrom, DateTime? dateTo, int challanStatus, string searchString, string compId,string challanType,
            out int totalRecords)
        {
            bool isApproved = false;
            if (challanStatus==1)
            {
                isApproved = true;
            }
            else
            {
                isApproved = false;
            }
                        
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var returnableChallanList =_returnableChallanRepository.Filter(x => x.CompId == compId&&x.ChllanType==challanType &&((x.ReturnableChallanRefId.Trim().Contains(searchString) || String.IsNullOrEmpty(searchString))
                && ((x.IsApproved == true || x.IsApproved == false)||((x.ChallanDate >= dateFrom || dateFrom==null) && ( x.ChallanDate <= dateTo || dateTo==null)))
                && ((x.ChallanDate >= dateFrom || dateFrom==null) && ( x.ChallanDate <= dateTo || dateTo==null) && (x.IsApproved == isApproved || challanStatus==0))));
            totalRecords =returnableChallanList.Count();
            switch (sort)
            {
                case "Messrs":
                    switch (sortdir)
                    {
                        case "DESC":
                            returnableChallanList =returnableChallanList
                                .OrderByDescending(r => r.ReturnableChallanId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            returnableChallanList = returnableChallanList
                                .OrderBy(r => r.ReturnableChallanId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    returnableChallanList = returnableChallanList
                        .OrderByDescending(r => r.ReturnableChallanId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return returnableChallanList.ToList();
        }

        public Maintenance_ReturnableChallan GetReturnableChallanByReturnableChallanId(long returnableChallanId, string compId)
        {
            return _returnableChallanRepository.FindOne( x => x.CompId == compId && x.ReturnableChallanId == returnableChallanId);
        }

     
        public int EditReturnableChallan(Maintenance_ReturnableChallan model)
        {
            int edited = 0;
            using (var transaction = new TransactionScope())
            {
               _returnableChallanDetailRepository.Delete(x => x.ReturnableChallanId == model.ReturnableChallanId);
                var retrurnableChallan=  _returnableChallanRepository.FindOne(x => x.ReturnableChallanId == model.ReturnableChallanId);
                retrurnableChallan.Messrs = model.Messrs;
                retrurnableChallan.Address = model.Address;
                retrurnableChallan.ChallanDate = model.ChallanDate;
                retrurnableChallan.EmployeeCardId = model.EmployeeCardId;
                retrurnableChallan.RefferancePerson = model.RefferancePerson;
                retrurnableChallan.Department = model.Department;
                retrurnableChallan.Designation = model.Designation;
                retrurnableChallan.Phone = model.Phone;
                edited = _returnableChallanRepository.Edit(retrurnableChallan);
                var detail = model.Maintenance_ReturnableChallanDetail.Select(x =>
                {
                    x.ReturnableChallanId = retrurnableChallan.ReturnableChallanId;
                    return x;
                });
                edited += _returnableChallanDetailRepository.SaveList(detail.ToList());
                transaction.Complete();
            }
            return edited;
        }

        public int SaveReturnableChallan(Maintenance_ReturnableChallan model)
        {
            model.CompId = PortalContext.CurrentUser.CompId;
            model.PreparedBy = PortalContext.CurrentUser.UserId;
            model.IsApproved = false;
          
            return _returnableChallanRepository.Save(model);
             
        }

        public int DeleteReturnableChallan(long returnableChallanId, string compId)
        {
            int deleted = 0;
            using (var transaction = new TransactionScope())
            {
                deleted += _returnableChallanDetailRepository.Delete(x => x.CompId == compId && x.ReturnableChallanId == returnableChallanId);
                deleted += _returnableChallanRepository.Delete(x => x.CompId == compId && x.ReturnableChallanId == returnableChallanId);
                transaction.Complete();
            }
            return deleted;
        }

        public List<VwReturnableChallan> GetReturnableChallanForReport(long returnableChallanId, string compId)
        {
            return _returnableChallanRepository.GetReturnableChallanForReport(returnableChallanId, compId);
        }

        public string GetReturnableChallanRefId(string challanType,string preefix)
        {
            var maxReturnableChallanRefId = _returnableChallanRepository.Filter(x => x.CompId == PortalContext.CurrentUser.CompId && x.ChllanType == challanType).Max(x => x.ReturnableChallanRefId.Substring(2)) ?? "0";
            return preefix + maxReturnableChallanRefId.IncrementOne().PadZero(4);
        }

        public Maintenance_ReturnableChallan GetReturnableChallanByReturnableChallanRefId(string returnableChallanRefId, string compId)
        {
            return _returnableChallanRepository.FindOne(x => x.CompId == compId && x.ReturnableChallanRefId == returnableChallanRefId);
        }

        public object GetRefNoBySearchCharacter(string searchCharacter,string challanType)
        {
            string comId = PortalContext.CurrentUser.CompId;
            return _returnableChallanRepository.Filter(x => x.CompId == comId &&x.ChllanType==challanType&& (x.ReturnableChallanRefId.Trim().Replace(" ", String.Empty)
               .ToLower().Contains(searchCharacter.Trim().Replace(" ", String.Empty).ToLower()))).Take(10);
        }

  
        public List<Maintenance_ReturnableChallan> GetApprovedReturnableChallanByPaging(int pageIndex, string sort, string sortdir, bool? isApproved,string compId,string challanType, out int totalRecords)
        {
            var index = pageIndex;
            var pageSize = AppConfig.PageSize;
            var returnableChallanList = _returnableChallanRepository.Filter(x => x.CompId == compId&&x.ChllanType==challanType && (x.IsApproved == isApproved || isApproved==null));
            totalRecords = returnableChallanList.Count();
            switch (sort)
            {
                case "Messrs":
                    switch (sortdir)
                    {
                        case "DESC":
                            returnableChallanList = returnableChallanList
                                .OrderByDescending(r => r.ReturnableChallanId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            returnableChallanList = returnableChallanList
                                .OrderBy(r => r.ReturnableChallanId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    returnableChallanList = returnableChallanList
                        .OrderByDescending(r => r.ReturnableChallanId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return returnableChallanList.ToList();
        }

        public int ApprovedReturnableChallan(long returnableChallanId, string compId)
        {
            
           Maintenance_ReturnableChallan returnableChallan = _returnableChallanRepository.FindOne(x => x.CompId == compId && x.ReturnableChallanId == returnableChallanId);
           returnableChallan.IsApproved = returnableChallan.IsApproved != true;
           returnableChallan.ApprovedBy =    returnableChallan.IsApproved==true? PortalContext.CurrentUser.UserId:null;
           return _returnableChallanRepository.Edit(returnableChallan);
        }

        public DataTable GetReturnableChallanInfo(DateTime? dateFrom, DateTime? dateTo,string challanType, int challanStatus, string compId)
        {
            return _returnableChallanRepository.GetReturnableChallanInfo(dateFrom, dateTo, challanType, challanStatus, compId);
        }
    }
}
