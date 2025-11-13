using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.BLL.IManager.IPayrollManager;
using SCERP.Common;
using SCERP.DAL.IRepository.IPayrollRepository;
using SCERP.Model.PayrollModel;

namespace SCERP.BLL.Manager.PayrollManager
{
    public class AdvanceIncomeTaxManager : IAdvanceIncomeTaxManager
    {
        private readonly IAdvanceIncomeTaxRepository _advanceIncomeTaxRepository;
        private readonly string _compId;
        public AdvanceIncomeTaxManager(IAdvanceIncomeTaxRepository advanceIncomeTaxRepository)
        {
            _advanceIncomeTaxRepository = advanceIncomeTaxRepository;
            _compId = PortalContext.CurrentUser.CompId;
        }
        public int DeleteAdvanceIncomeTax(int advanceTaxId)
        {
            return _advanceIncomeTaxRepository.Delete(x => x.AdvanceTaxId == advanceTaxId);
        }

        public int EditAdvanceIncomeTax(AdvanceIncomeTax model)
        {
            AdvanceIncomeTax advanceIncomeTax = _advanceIncomeTaxRepository.FindOne(x => x.AdvanceTaxId == model.AdvanceTaxId);

            advanceIncomeTax.AdvanceTaxId = model.AdvanceTaxId;
            advanceIncomeTax.EmployeeId = model.EmployeeId;
            advanceIncomeTax.EmployeeCardId = model.EmployeeCardId;
            advanceIncomeTax.Amount = model.Amount;
            advanceIncomeTax.FromDate = model.FromDate;
            advanceIncomeTax.ToDate = model.ToDate;
            advanceIncomeTax.CreatedDate = model.CreatedDate;
            advanceIncomeTax.CreatedBy = model.CreatedBy;
            advanceIncomeTax.EditedDate = model.EditedDate;
            advanceIncomeTax.EditedBy = model.EditedBy;
            advanceIncomeTax.IsActive = model.IsActive;


            return _advanceIncomeTaxRepository.Edit(advanceIncomeTax);
        }

        public AdvanceIncomeTax GetAdvanceIncomeTaxId(int advanceTaxId)
        {
            //string compId = PortalContext.CurrentUser.CompId;
            var itemList = _advanceIncomeTaxRepository.Filter(x => x.AdvanceTaxId == advanceTaxId).FirstOrDefault(x => x.AdvanceTaxId == advanceTaxId);
            return itemList;
        }

        public List<AdvanceIncomeTax> GetAllAdvanceIncomeTaxs()
        {
            var advanceIncomeTaxList = _advanceIncomeTaxRepository.All();
            return advanceIncomeTaxList.ToList();
        }

        public List<AdvanceIncomeTax> GetAllAdvanceIncomeTaxsByPaging(AdvanceIncomeTax model, out int totalRecords)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            var aitList = _advanceIncomeTaxRepository.Filter(x => (x.EmployeeCardId.Trim().Contains(model.EmployeeCardId.Trim()) || String.IsNullOrEmpty(model.EmployeeCardId)) && (x.FromDate>=model.FromDate ||model.FromDate==null)&& (x.ToDate <= model.ToDate || model.ToDate == null));
            totalRecords = aitList.Count();
            switch (model.sort)
            {
                case "AdvanceTaxId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            aitList = aitList
                                 .OrderByDescending(r => r.AdvanceTaxId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            aitList = aitList
                                 .OrderBy(r => r.AdvanceTaxId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                case "EmployeeCardId":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            aitList = aitList
                                 .OrderByDescending(r => r.EmployeeCardId)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            aitList = aitList
                                 .OrderBy(r => r.EmployeeCardId)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;


               

                default:
                    aitList = aitList
                        .OrderByDescending(r => r.AdvanceTaxId)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }

            return aitList.ToList();
        }

        public string GetNewAdvanceIncomeTaxRefId(string prifix)
        {
            throw new NotImplementedException();
        }

        public bool IsStyleAdvanceIncomeTaxExist(AdvanceIncomeTax model)
        {
            return _advanceIncomeTaxRepository.Exists(x => x.AdvanceTaxId == model.AdvanceTaxId && x.EmployeeId == model.EmployeeId && x.EmployeeCardId == model.EmployeeCardId && x.Amount == model.Amount && x.FromDate == model.FromDate && x.ToDate == model.ToDate && x.CreatedDate==model.CreatedDate && x.CreatedBy==model.CreatedBy && x.EditedDate==model.EditedDate && x.EditedBy==model.EditedBy && x.IsActive==model.IsActive);
        }

        public int SaveAdvanceIncomeTax(AdvanceIncomeTax model)
        {
            //model.CompId = PortalContext.CurrentUser.CompId;

            return _advanceIncomeTaxRepository.Save(model);
        }
    }
}
