using SCERP.BLL.IManager.ICommercialManager;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class TNAController : BaseController
    {
        private readonly ICommTNAManager _commTNAManager;
        private readonly ISalesContactManager _salesContactManager;

        public TNAController(ICommTNAManager commTNAManager, ISalesContactManager salesContactManager)
        {

            _commTNAManager = commTNAManager;
            _salesContactManager = salesContactManager;

        }
        
        // GET: Commercial/TNA
        public ActionResult Index(CommTNAViewModel model)
        {
            ModelState.Clear();
            model.CommSalesContacts = _salesContactManager.GetAllSalesContacts();
            model.CommTNAs = _commTNAManager.GetLCWiseTna(model.LCRefId);
            return View(model);
        }

        [HttpPost]
        // [AjaxAuthorize(Roles = "tnaxl-2,tnaxl-3")]
        public ActionResult UpdateTna(int commTnaRowId, string key, string value)
        {
            int updated = _commTNAManager.UpdateTna(PortalContext.CurrentUser.CompId, commTnaRowId, key, value);
            return Json(true);
        }

        [HttpPost]
        //[AjaxAuthorize(Roles = "tnaxl-3")]
        public ActionResult AddRow(CommTNAViewModel model)
        {
            int updated = _commTNAManager.AddRows(model.RowNumber, model.LCRefId, PortalContext.CurrentUser.CompId);
            return Reload();
        }

        [HttpPost]
        //[AjaxAuthorize(Roles = "tnaxl-3")]
        public ActionResult RemoveRow(CommTNAViewModel model)
        {
            int updated = _commTNAManager.RemoveRow(model.RowNumber, model.LCRefId, PortalContext.CurrentUser.CompId);
            return Reload();
        }
    }
}