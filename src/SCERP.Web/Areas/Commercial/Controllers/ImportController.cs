using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IHRMManager;
using SCERP.Common;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using System;

using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class ImportController : BaseController
    {

       
        private readonly ILcManager _lcManager;
        private readonly ICountryManager _countryManager;
        private readonly ISalesContactManager _salesContactManager;
        private readonly IImportManager _importManager;

        public ImportController(IImportManager importManager,IExportManager exportManager, ILcManager lcManager, ICountryManager countryManager, ISalesContactManager salesContactManager)
        {
           
            _lcManager = lcManager;
            _countryManager = countryManager;
            _salesContactManager = salesContactManager;
            _importManager = importManager;
        }
        // GET: Commercial/Import
        public ActionResult Index(ImportViewModel model)
        {
            ModelState.Clear();
            int totalRecords=0;
            model.CommImports = _importManager.GetAllImportsByPaging( out totalRecords, model.CommImport);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult Edit(ImportViewModel model)
        {
            ModelState.Clear();

            if (model.CommImport.ImportId > 0)
            {
                model.CommImport = _importManager.GetImportById(model.CommImport.ImportId);
                //model.CommExport.InvoiceValue = model.CommExport.InvoiceValue == null ? 0 : model.CommExport.InvoiceValue;
                //model.CommExport.RealizedValue = model.CommExport.RealizedValue == null ? 0 : model.CommExport.RealizedValue;
                //model.CommExport.ShortFallAmount = model.CommExport.InvoiceValue - model.CommExport.RealizedValue;
            }
            else
            {
                //model.CommExport.ExportRefId = _exportManager.GetNewExportRefId();
            }

            model.CommLcInfos = _lcManager.GetAllLcInfos();
            //model.CommSalesContacts = _salesContactManager.GetSalesContacts(model.CommImport.LCNo);
            return View(model);
        }


        public ActionResult Save(ImportViewModel model)
        {
            var saveIndex = 0;
            try
            {
                saveIndex = model.CommImport.ImportId > 0 ? _importManager.EditImport(model.CommImport) : _importManager.SaveImport(model.CommImport);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return saveIndex > 0 ? Reload() : ErrorMessageResult();
        }

        

        public ActionResult Delete(ImportViewModel model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = _importManager.DeleteImport(model.CommImport);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }
            return deleteIndex > 0 ? Reload() : ErrorResult("Delete Failed");
        }


    }
}