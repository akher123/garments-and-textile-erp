using System;
using System.Web.Mvc;
using SCERP.BLL.Manager.MerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class SupplierCompanyController : BaseController
    {
    

        [AjaxAuthorize(Roles = "suppliercompany-1,suppliercompany-2,suppliercompany-3")]
        public ActionResult Index(SupplierViewModel model)
         {
             int totalRecors = 0;
             model.SupplierCompanies = SupplierCompanyManager.GetSupplierCompanyByPaging(model, out totalRecors);
             model.TotalRecords = totalRecors;
             return View(model);
        }
        [AjaxAuthorize(Roles = "suppliercompany-2,suppliercompany-3")]
        public ActionResult Edit(Mrc_SupplierCompany supplierCompany)
        {
            ModelState.Clear();
            if (supplierCompany.SupplierCompanyId > 0)
            {
                supplierCompany = SupplierCompanyManager.GetSupplierCompanyById(supplierCompany.SupplierCompanyId);
            }
            return View(supplierCompany);
        }
          [AjaxAuthorize(Roles = "suppliercompany-2,suppliercompany-3")]
        public ActionResult Save(Mrc_SupplierCompany supplierCompany)
        {
            var saveIndex = 0;
              try
              {
                  saveIndex = supplierCompany.SupplierCompanyId > 0 ? SupplierCompanyManager.EditSupplierCompany(supplierCompany) : SupplierCompanyManager.SaveSupplierCompany(supplierCompany);
              }
              catch (Exception exception)
              {

                return  ErrorResult(exception.Message);
              }
          
            return saveIndex > 0 ? Reload() : ErrorMessageResult();

        }
          [AjaxAuthorize(Roles = "suppliercompany-3")]
        public ActionResult Delete(int? id)
        {
            var saveIndex = SupplierCompanyManager.DeleteSupplierCompany(id);
            return saveIndex > 0 ? Reload() : ErrorResult("not delele");
        }

	}
}