using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.Common;
using SCERP.Model.CommercialModel;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.MIS.Controllers
{
    public class MisCommercialReportController : BaseController
    {
       
      
        private readonly IOmBuyerManager _omBuyerManager;

        public MisCommercialReportController(IOmBuyerManager _omBuyerManager)
        {

            this._omBuyerManager = _omBuyerManager;
          
        }
        public ActionResult ExportImportPerformance(LcViewModel model)
        {
            try
            {
                ModelState.Clear();
                IEnumerable lcTypeList = from LcType lcType in Enum.GetValues(typeof(LcType))
                                         select new { Id = (int)lcType, Name = lcType.ToString() };
                model.Buyers = _omBuyerManager.GetAllBuyers();
                model.LcTypes = lcTypeList;  
               
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
            }
            return View(model);
        }
	}
}