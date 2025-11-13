using System;
using System.Web.Mvc;
using SCERP.BLL.IManager.IMerchandisingManager;
using SCERP.Common;
using SCERP.Model;
using SCERP.Web.Areas.Merchandising.Models.ViewModel;

namespace SCERP.Web.Areas.Merchandising.Controllers
{
    public class AgentController : BaseMerchandisingController
    {
        private IAgentManager agentManager;
        public AgentController(IAgentManager agentManager)
        {
            this.agentManager = agentManager;
        }
        [AjaxAuthorize(Roles = "agent-1,agent-2,agent-3")]
        public ActionResult Index(AgentViewModel model)
        {
            ModelState.Clear();
            var totalRecords = 0;
            model.Agents = agentManager.GetAgentByPaging(model, out totalRecords);
            model.TotalRecords = totalRecords;
            return View(model);
        }
         [AjaxAuthorize(Roles = "agent-2,agent-3")]
         [HttpGet]
        public ActionResult Edit(AgentViewModel model)
        {
            ModelState.Clear();
            model.Countries = CountryManager.GetAllCountries();
            if (model.AgentId > 0)
            {
                var agent = agentManager.GetAgentById(model.AgentId);
                model.AgentId = agent.AgentId;
                model.AgentName = agent.AgentName;
                model.AgentRefId = agent.AgentRefId;
                model.Address1 = agent.Address1;
                model.Address2 = agent.Address2;
                model.Address3 = agent.Address3;
                model.Address3 = agent.Address3;
                model.AType = agent.AType;
                model.CountryId = agent.CountryId;
                model.CityId = agent.CityId;
                model.Phone = agent.Phone;
                model.Fax = agent.Fax;
                model.EMail = agent.EMail;
                model.Cities = CityManager.GetCityByCountry(model.CountryId);
            }
            else
            {
                model.AgentRefId = agentManager.GetNewAgentRefId();
            }
            return View(model);
        }
        [AjaxAuthorize(Roles = "agent-2,agent-3")]
        [HttpPost]
        public ActionResult Save(OM_Agent model)
        {
            var index = 0;
            var errorMessage = "";
            try
            {
                index = model.AgentId > 0 ? agentManager.EditAgent(model) : agentManager.SaveAgent(model);
            }
            catch (Exception exception)
            {
                errorMessage = exception.Message;
                Errorlog.WriteLog(exception);
            }
            if (!model.IsSearch)
            {
                return index > 0 ? Reload() : ErrorResult("Agent information save fail " + errorMessage);
            }
            else
            {
                  var agentList=   agentManager.GetAgents();
                  return Json(agentList, JsonRequestBehavior.AllowGet);
            }
          
        }
       [AjaxAuthorize(Roles = "agent-3")]
        public ActionResult Delete(OM_Agent model)
        {

            var saveIndex = agentManager.DeleteAgent(model.AgentRefId);
            if (saveIndex == -1)
            {
                return ErrorResult("Could not possible to delete Agent because of it's all ready used in buyer Order");
            }

            return saveIndex > 0 ? Reload() : ErrorResult("Delete Fail");
        }

        [HttpPost]
        public JsonResult CheckExistingAgent(OM_Agent model)
        {
            var isExist = !agentManager.CheckExistingAgent(model);
            return Json(isExist, JsonRequestBehavior.AllowGet);
        }
	}
}