using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.Model.CommercialModel;
using SCERP.Web.Areas.Commercial.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Commercial.Controllers
{
    public class PackingCreditController : BaseController
    {
        private readonly IPackingCreditManager _packingCreditManager;

        public PackingCreditController(IPackingCreditManager packingCreditManager)
        {
            _packingCreditManager = packingCreditManager;
        }
        public ActionResult Index(int lcId)
        {
            PackingCreditViewModel model = new PackingCreditViewModel
            {
                PackingCredit =
                {
                    LcId = lcId,
                    CreditDate = DateTime.Now
                },
                PackingCredits = _packingCreditManager.GetPakingCredits(lcId)
            };
            return View(model);
        }

        public ActionResult Save(PackingCreditViewModel model)
        {

            int saved = 0;
            try
            {
                if (model.PackingCredit.PackingCreditId > 0)
                {

                    model.PackingCredit.EditedBy = PortalContext.CurrentUser.UserId;
                    model.PackingCredit.EditedDate = DateTime.Now;
                    saved = _packingCreditManager.EditPackingCredit(model.PackingCredit);
                }
                else
                {
                    model.PackingCredit.IsAcive = true;
                    model.PackingCredit.CreatedDate = DateTime.Now;
                    model.PackingCredit.CreatedBy = PortalContext.CurrentUser.UserId.GetValueOrDefault();
                    saved = _packingCreditManager.SavePackingCredit(model.PackingCredit);
                }

            }
            catch (Exception e)
            {
                Errorlog.WriteLog(e);
                return ErrorResult(e.Message);
            }


            if (saved > 0)
            {

                model.PackingCredits = _packingCreditManager.GetPakingCredits(model.PackingCredit.LcId);
                return PartialView("~/Areas/Commercial/Views/PackingCredit/_List.cshtml", model);
            }
            else
            {
                return ErrorResult("Save Failed!Plse contact with vendor");
            }

        }

        public ActionResult Edit(int packingCreditId)
        {

            PackingCreditViewModel model = new PackingCreditViewModel
            {
                PackingCredit = _packingCreditManager.GetPakingCreditById(packingCreditId) ?? new CommPackingCredit()
                {
                    CreditDate = DateTime.Now
                }
            };
            return PartialView("~/Areas/Commercial/Views/PackingCredit/_Edit.cshtml", model);
        }

        public ActionResult Refresh(int lcId)
        {

            PackingCreditViewModel model = new PackingCreditViewModel
            {
                PackingCredit =
                {
                    LcId = lcId,
                    CreditDate = DateTime.Now
                }
            };
            return PartialView("~/Areas/Commercial/Views/PackingCredit/_Edit.cshtml", model);
        }

        public ActionResult Delete(int packingCreditId, int lcId)
        {
            PackingCreditViewModel model = new PackingCreditViewModel();
            int delete = _packingCreditManager.DeletePackingCredit(packingCreditId);
            if (delete > 0)
            {

                model.PackingCredits = _packingCreditManager.GetPakingCredits(lcId);
                return PartialView("~/Areas/Commercial/Views/PackingCredit/_List.cshtml", model);
            }
            else
            {
                return ErrorResult("Delete Failed!Please contact with vendor");
            }

        }
    }
}