using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.Common;
using SCERP.Model.CommonModel;
using SCERP.Web.Areas.Common.Models.ViewModel;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.Production.Controllers
{
    public class DyeingFactoryController : BaseController
    {
       private readonly IPartyManager _partyManager;
       public DyeingFactoryController(IPartyManager partyManager)
        {
            _partyManager = partyManager;
        }
          [AjaxAuthorize(Roles = "dyeingfactory-1,dyeingfactory-2,dyeingfactory-3")]
        public ActionResult Index(PartyViewModel model)
        {

            var totalRecords = 0;
            ModelState.Clear();
            model.PType = "D";
            model.Parties = _partyManager.GetPartiesByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);

        }
        [AjaxAuthorize(Roles = "dyeingfactory-2,dyeingfactory-3")]
        public ActionResult Edit(PartyViewModel model)
        {
            ModelState.Clear();
            if (model.PartyId > 0)
            {
                Party party = _partyManager.GetPartyById(model.PartyId);
                model.Name = party.Name;
                model.PartyRefNo = party.PartyRefNo;
                model.Address = party.Address;
                model.Email = party.Email;
                model.Phone = party.Phone;
                model.ContactPersonName = party.ContactPersonName;
                model.ContactPhone = party.ContactPhone;

            }
            
            return View(model);
        }
        [AjaxAuthorize(Roles = "dyeingfactory-2,dyeingfactory-3")]
        public ActionResult Save(Party model)
        {
            var index = 0;
            model.PType = "D";
            index = model.PartyId > 0 ? _partyManager.EditParty(model) : _partyManager.SaveParty(model);
            return index > 0 ? Reload() : ErrorResult("Party save fail");
        }

        public ActionResult Delete(long partyId)
        {
            int deleteIndex = _partyManager.DeleteParty(partyId);
            if (deleteIndex>0)
            {
                return Reload();
            }
            else
            {
                return ErrorResult("Failed To Delete Factory");
            }
        }
	}
}