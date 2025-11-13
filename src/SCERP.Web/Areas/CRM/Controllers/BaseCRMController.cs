using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.BLL.IManager.ICRMManager;
using SCERP.BLL.IManager.IPlanningManager;
using SCERP.BLL.Manager.PlanningManager;
using SCERP.Web.Controllers;

namespace SCERP.Web.Areas.CRM.Controllers
{
    public class BaseCrmController : BaseController
    {
        #region CRM

        //public IPlanningReportManager PlanningReportManager
        //{
        //    get { return Manager.PlanningReportManager; }
        //}

        public ICRMReportManager CRMReportManager
        {
            get { return Manager.CrmReportManager; }
        }

        #endregion
    }
}