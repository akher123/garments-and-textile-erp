using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model.CRMModel;
using System.ComponentModel.DataAnnotations;
using SCERP.Model;

namespace SCERP.Web.Areas.CRM.Models.ViewModels
{
    public class FeedbackViewModel : CRMFeedback
    {
        public FeedbackViewModel()
        {
            Feedback = new List<CRMFeedback>();
            IsSearch = true;
        }

        public List<CRMFeedback> Feedback { get; set; }


        public List<Module> Modules { get; set; }
        public List<SelectListItem> ModuleSelectListItem
        {
            get { return new SelectList(Modules, "Id", "ModuleName").ToList(); }
        }


        public IEnumerable FeedbackStatus { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> FeedbackStatusSelectListItem
        {
            get { return new SelectList(FeedbackStatus, "Id", "Name"); }
        }


        public string PhotographPath { get; set; }

        public string SearchKey
        {
            get;
            set;
        }
    }
}