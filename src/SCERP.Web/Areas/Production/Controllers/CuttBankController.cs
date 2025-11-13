using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.BLL.IManager.IProductionManager;
using SCERP.Web.Areas.Production.Models;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class CuttBankController : BaseController
    {
        private readonly ICutBankManager _cutBankManager;
        private readonly IOmBuyerManager _buyerManager;
        private readonly ICuttingProcessStyleActiveManager _cuttingProcessStyleActive;
        public CuttBankController( ICuttingProcessStyleActiveManager cuttingProcessStyleActive,ICutBankManager cutBankManager, IOmBuyerManager buyerManager)
        {
            _cutBankManager = cutBankManager;
            _buyerManager = buyerManager;
            _cuttingProcessStyleActive = cuttingProcessStyleActive;
        }

        public ActionResult Index(CutBangkViewModel model)
        {
            ModelState.Clear();
            model.Buyers = _buyerManager.GetAllBuyers();
            if (!model.IsSearch)
            {
                model.IsSearch = true;
                return View(model);
            }
            else
            {
                model.CutBanks = _cutBankManager.GetAllCutBank(model.OrderStyleRefId); 
            }
            model.OrderList = _cuttingProcessStyleActive.GetOrderByBuyer(model.BuyerRefId);
            model.StyleList = _cuttingProcessStyleActive.GetStyleByOrderNo(model.OrderNo);
            return View(model);
        }

        public ActionResult UpdateCutBank(CutBangkViewModel model)
        {
            int updated = _cutBankManager.UpdateCutBank(model.OrderStyleRefId);
            var message = "";
            message = updated>0 ? "CuttBank Update Successfully" : "Update Failed";
            return ErrorResult(message);
        }

	}
}