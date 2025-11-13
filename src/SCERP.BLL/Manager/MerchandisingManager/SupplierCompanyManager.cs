using System;
using System.Collections.Generic;
using System.Linq;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.DAL;
using SCERP.DAL.IRepository.IMerchandisingRepository;
using SCERP.DAL.Repository.MerchandisingRepository;
using SCERP.Model;

namespace SCERP.BLL.Manager.MerchandisingManager
{
    public class SupplierCompanyManager : ISupplierCompanyManager
    {
        private readonly ISupplierCompanyRepository _supplierCompanyRepository = null;
        public SupplierCompanyManager(SCERPDBContext context)
        {
            _supplierCompanyRepository = new SupplierCompanyRepository(context);
        }

        public List<Mrc_SupplierCompany> GetAllSupplierCompany()
        {

            var supplierCompanies = new List<Mrc_SupplierCompany>();
            try
            {
                supplierCompanies = _supplierCompanyRepository.Filter(x=>x.IsActive).OrderBy(p=>p.CompanyName).ToList();
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);

            }
            return supplierCompanies;
        }

        public Mrc_SupplierCompany GetSupplierCompanyById(int supplierCompanyId)
        {
            var supplierCompany = new Mrc_SupplierCompany();

            try
            {
                supplierCompany = _supplierCompanyRepository.FindOne(x => x.SupplierCompanyId == supplierCompanyId && x.IsActive);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return supplierCompany;
        }

        public int EditSupplierCompany(Mrc_SupplierCompany supplierCompany)
        {
            var editIndex = 0;
            bool isExist =
                _supplierCompanyRepository.Exists(
                    x =>
                        x.CompanyName.Trim().Replace(" ", "").ToLower() ==
                        supplierCompany.CompanyName.Trim().Replace(" ", "").ToLower()&&x.SupplierCompanyId!=supplierCompany.SupplierCompanyId);
            if (isExist)
            {
                throw new ArgumentException(message: "Supplier Name Already exist");
            }
            else
            {
                var supplierCompanyObj = _supplierCompanyRepository.FindOne(x => x.SupplierCompanyId == supplierCompany.SupplierCompanyId && x.IsActive == true);
                supplierCompanyObj.CompanyName = supplierCompany.CompanyName;
                supplierCompanyObj.ContactName = supplierCompany.ContactName;
                supplierCompanyObj.SupplierCode = supplierCompany.SupplierCode;
                supplierCompanyObj.Address = supplierCompany.Address;
                supplierCompanyObj.Phone = supplierCompany.Phone;
                supplierCompanyObj.Email = supplierCompany.Email;
                supplierCompanyObj.Fax = supplierCompany.Fax;
                supplierCompanyObj.Web = supplierCompany.Web;
                supplierCompanyObj.EditedDate = DateTime.Now;
                supplierCompanyObj.EditedBy = PortalContext.CurrentUser.UserId;
                editIndex = _supplierCompanyRepository.Edit(supplierCompanyObj);
            }
         
           
        

            return editIndex;
        }

        public int SaveSupplierCompany(Mrc_SupplierCompany supplierCompany)
        {
            var saveIndex = 0;
            bool isExist =
              _supplierCompanyRepository.Exists(
                  x =>
                      x.CompanyName.Trim().Replace(" ", "").ToLower() == supplierCompany.CompanyName.Trim().Replace(" ", "").ToLower());
            if (isExist)
            {
                throw new ArgumentException(message: "Supplier Name Already exist");
            }
            else
            {
                supplierCompany.CreatedDate = DateTime.Now;
                supplierCompany.CreatedBy =PortalContext.CurrentUser.UserId;
                supplierCompany.IsActive = true;
                saveIndex = _supplierCompanyRepository.Save(supplierCompany); 
            }
            return saveIndex;
        }

        public int DeleteSupplierCompany(int? id)
        {
            var deleteIndex = 0;
            try
            {
                var supplierCompanyObj = _supplierCompanyRepository.FindOne(x => x.SupplierCompanyId == id && x.IsActive == true);
                supplierCompanyObj.IsActive = false;
                deleteIndex = _supplierCompanyRepository.Edit(supplierCompanyObj);
            }
            catch (Exception exception)
            {

                Errorlog.WriteLog(exception);
            }
            return deleteIndex;
        }



        public List<Mrc_SupplierCompany> GetSupplierCompanyByPaging(Mrc_SupplierCompany model, out int totalRecors)
        {
            var index = model.PageIndex;
            var pageSize = AppConfig.PageSize;
            IQueryable<Mrc_SupplierCompany> suppliers = _supplierCompanyRepository.Filter(x => x.IsActive && (x.CompanyName.Replace(" ", "").ToLower().Contains(model.SearchString.Trim().ToLower()) || model.SearchString == null));
            totalRecors = suppliers.Count();
            switch (model.sort)
            {
                case "CompanyName":
                    switch (model.sortdir)
                    {
                        case "DESC":
                            suppliers = suppliers
                                .OrderByDescending(r => r.CompanyName)
                                .Skip(index * pageSize)
                                .Take(pageSize);

                            break;
                        default:
                            suppliers = suppliers
                                .OrderBy(r => r.CompanyName)
                                .Skip(index * pageSize)
                                .Take(pageSize);
                            break;
                    }
                    break;

                default:
                    suppliers = suppliers
                        .OrderBy(r => r.CompanyName)
                        .Skip(index * pageSize)
                        .Take(pageSize);
                    break;
            }
            return suppliers.ToList();
        }
    }
}
