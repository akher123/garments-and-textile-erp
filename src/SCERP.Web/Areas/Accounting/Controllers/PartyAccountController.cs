using SCERP.BLL.IManager.ICommonManager;
using SCERP.Model.CommonModel;
using SCERP.Web.Areas.Accounting.Models.ViewModels;
using SCERP.Web.Controllers;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Accounting.Controllers
{
    public class PartyAccountController : BaseController
    {
        private readonly IPartyManager partyManager;
        public PartyAccountController(IPartyManager partyManager)
        {
            this.partyManager = partyManager;
        }
        public ActionResult Index(PartyAccountViewModel model)
        {

            var totalRecords = 0;
            ModelState.Clear();
            model.Party.PType = model.Party.PType??"P";
            model.Parties = partyManager.GetVwPartiesByPaging(model.PageIndex,model.Party.PType, model.SearchString, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }

        public ActionResult UpdateParty(PartyAccountViewModel model)
        {
            int updated = partyManager.UpdateParty(model.GlId, model.Party.PartyId,model.PrType);
            return Reload();
        }
        public ActionResult Update(long partyId)
        {
            PartyAccountViewModel model = new PartyAccountViewModel();
            VwParty party = partyManager.GetPartyViewById(partyId);
            model.Party = party;
            return View(model);
        }

        
    }
}