using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.Model.CommonModel;
using SCERP.Web.Areas.Common.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Common.Controllers
{
    public class PartyController : BaseController
    {
        private readonly IPartyManager partyManager;
        public PartyController(IPartyManager partyManager)
        {
            this.partyManager = partyManager;
        }
        [AjaxAuthorize(Roles = "party-1,party-2,party-3")]
        public ActionResult Index(PartyViewModel model)
        {

            var totalRecords = 0;
            ModelState.Clear();
            model.PType = "P";
            model.Parties = partyManager.GetPartiesByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
           
        }
          [AjaxAuthorize(Roles = "party-2,party-3")]
        public ActionResult Edit(PartyViewModel model)
        {
            ModelState.Clear();
            if (model.PartyId > 0)
            {
                Party party = partyManager.GetPartyById(model.PartyId);
                model.Name = party.Name;
                model.PartyRefNo = party.PartyRefNo;
                model.Address = party.Address;
                model.Email = party.Email;
                model.Phone = party.Phone;
                model.ContactPersonName = party.ContactPersonName;
                model.ContactPhone = party.ContactPhone;

            }
            else
            {
                model.PartyRefNo = partyManager.GetNewPartyRef();
            }
            return View(model);
        }
          [AjaxAuthorize(Roles = "party-2,party-3")]
        public ActionResult Save(Party model)
        {
            var index = 0;
            model.PType = "P";
            index = model.PartyId > 0 ? partyManager.EditParty(model) : partyManager.SaveParty(model);
            return index > 0 ? Reload() : ErrorResult("Party save fail");
        }
	}
}