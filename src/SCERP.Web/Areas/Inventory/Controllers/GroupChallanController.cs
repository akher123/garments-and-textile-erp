using Microsoft.Reporting.WebForms;
using SCERP.BLL.IManager.ICommonManager;
using SCERP.BLL.IManager.IInventoryManager;
using SCERP.Common;
using SCERP.Web.Areas.Inventory.Models.ViewModels;
using SCERP.Web.Controllers;
using SCERP.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SCERP.Web.Areas.Inventory.Controllers
{
    public class GroupChallanController : BaseController
    {
      private readonly IGroupChallanManager _groupChallanManager;
        private IPartyManager _partyManager;
        private IAdvanceMaterialIssueManager _advanceMaterialIssueManager;
      public GroupChallanController(IGroupChallanManager groupChallanManager, IPartyManager partyManager, IAdvanceMaterialIssueManager advanceMaterialIssueManager)
      {
          _groupChallanManager = groupChallanManager;
          _partyManager = partyManager;
          _advanceMaterialIssueManager = advanceMaterialIssueManager;
      }
          [AjaxAuthorize(Roles = "groupchallan-1,groupchallan-2,groupchallan-3")]
        public ActionResult Index(GroupChallanViewModel model)
        {
            try
            {
                var totalRecords = 0;
                model.GroupChallans = _groupChallanManager.GetAllGroupChallanByPaging(model.PageIndex, model.sort, model.sortdir, out totalRecords, model.SearchString);
                model.TotalRecords = totalRecords;
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            return View(model);
        }
            [AjaxAuthorize(Roles = "groupchallan-2,groupchallan-3")]
        public ActionResult Edit(GroupChallanViewModel model)
        {
            ModelState.Clear();
            try
            {
                if (model.GroupChallan.GroupChallanId > 0)
                {
                     model.GroupChallan = _groupChallanManager.GetGroupChallanById(model.GroupChallan.GroupChallanId, PortalContext.CurrentUser.CompId);
                }
                else
                {
                    model.GroupChallan.RefId = _groupChallanManager.GetNewRefId();
                    model.GroupChallan.GDate = DateTime.Now;
                }
                model.Parties = _partyManager.GetParties("P");
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Retrive Data :" + exception);
            }
            
            return View(model);
        }
        [HttpPost]
        [AjaxAuthorize(Roles = "groupchallan-2,groupchallan-3")]
        public ActionResult Save(GroupChallanViewModel model)
        {
            ModelState.Clear();
            var index = 0;
            try
            {
                    model.GroupChallan.GType = (int)ActionType.YarnDelivery;
                    index = model.GroupChallan.GroupChallanId > 0 ? _groupChallanManager.EditGroupChallan(model.GroupChallan) : _groupChallanManager.SaveGroupChallan(model.GroupChallan);
                
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Failed to Save/Edit  :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Failed to Save/Edit  !");
        }

        [HttpGet]
        [AjaxAuthorize(Roles = "groupchallan-3")]
        public ActionResult Delete(int groupChallanId)
        {
            var index = 0;
            try
            {
                index = _groupChallanManager.DeleteGroupChallan(groupChallanId);
                if (index == -1)// This hour Id used by another table
                {
                    return ErrorResult("Could not possible to delete hour because of it's already used.");
                }
            }
            catch (Exception exception)
            {
                Errorlog.WriteLog(exception);
                return ErrorResult("Fail To Delete :" + exception);
            }
            return index > 0 ? Reload() : ErrorResult("Fail To Delete !");
        }

        public ActionResult GeChallanListByPartyId(int partyId)
        {
            
             GroupChallanViewModel model=new GroupChallanViewModel();
             model.GroupChallan.MaterialIssues= _advanceMaterialIssueManager.GeChallanListByPartyId(partyId);
             return PartialView("~/Areas/Inventory/Views/GroupChallan/_ChallanList.cshtml", model);
        }

        public ActionResult GroupChallanReport(int groupChallanId)
        {
            string reportName = "YarnCombinedDeliveryChallan";
            var reportParams = new List<ReportParameter> { new ReportParameter("groupChallanId", groupChallanId.ToString()),
                 new ReportParameter("CompId", PortalContext.CurrentUser.CompId),new ReportParameter("HostingServerAddress", AppConfig.HostingServerAddress) };
            return ReportExtension.ToSsrsFile(ReportType.PDF, reportName, reportParams);
        }
	}
}