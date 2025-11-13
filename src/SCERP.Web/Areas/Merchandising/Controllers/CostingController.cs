using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model.MerchandisingModel;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;
using SCERP.Web.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class CostingController : BaseController
    {
        private IMCostingManager _costingManager;

        public CostingController(IMCostingManager costingManager)
        {
            _costingManager = costingManager;
        }

        public ActionResult Index(MCostingViewModel model)
        {
            var totalRecords = 0;
            ModelState.Clear();
            model.MCostings = _costingManager.GetCostByPaging(model.Costing,model.PageIndex,model.sort,model.sortdir, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult Edit(MCostingViewModel model)
        {
            ModelState.Clear();
            if (model.CostingId > 0)
            {
                var mCosting = _costingManager.GetCostById(model.CostingId);
                model.Costing = mCosting;
            }
            return View("~/Areas/Merchandising/Views/Costing/Edit.cshtml",model);
        }

        public ActionResult Save(MCostingViewModel model)
        {
           
            if (model.Costing.CostingId>0)
            {
                OM_Costing mCosting = _costingManager.GetCostById(model.Costing.CostingId);
                model.Costing.BuyerName = mCosting.BuyerName;
            }
            _costingManager.SaveCost(model.Costing);
            return RedirectToAction("Edit",new{ CostingId= model.Costing.CostingId });
        }

        public ActionResult Delete(MCostingViewModel model)
        {
            var deleteIndex = 0;
            try
            {
                deleteIndex = _costingManager.DeleteCost(model.Costing.CostingId);
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult(exception.Message);
            }

            return deleteIndex > 0 ? Reload() : ErrorResult("Delete Fail");
        }

        public ActionResult UpdateCosting(int costingId,string fieldName,string value)
        {
           int index= _costingManager.UpdateCosting(costingId, fieldName, value);
           OM_Costing mCosting = _costingManager.GetUpdatedCosting(costingId);
           return Json(new {data= mCosting ,status= index }, JsonRequestBehavior.AllowGet);
        }
       


    }
}


