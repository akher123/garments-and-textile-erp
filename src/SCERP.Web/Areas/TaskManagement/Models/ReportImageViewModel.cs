using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.TaskManagementModel;
using System.Web;
using System.Web.Mvc;

namespace SCERP.Web.Areas.TaskManagement.Models
{
    public class ReportImageViewModel : TmReportImageInfo
    {
        public ReportImageViewModel()
        {
            ReportImage = new TmReportImageInfo();
            ReportImages = new List<TmReportImageInfo>();
            Task = new TmTask();
            Tasks = new List<TmTask>();
            Modules = new List<TmModule>();
            Subjects = new List<TmSubject>();
            TaskInformations = new List<vwTmTaskInformation>();
            Assignees = new List<TmAssignee>();
            TaskStatusList = new List<TmTaskStatus>();
            TaskTypes = new List<TmTaskType>();
        }
        //public HttpPostedFileBase File { get; set; }
        public int ModuleId { get; set; }
        public List<TmReportImageInfo> ReportImages { get; set; }
        public TmReportImageInfo ReportImage { get; set; }
        public List<TmReportImageInfo> ReportImageList { get; set; }
        public List<vwTmTaskInformation> TaskInformations { get; set; }
        public List<TmTask> Tasks { get; set; }
        public TmTask Task { get; set; }
        public List<TmModule> Modules { get; set; }
        public List<TmSubject> Subjects { get; set; }
        public string SubjectName { get; set; }
        public List<TmAssignee> Assignees { get; set; }
        public List<TmTaskStatus> TaskStatusList { get; set; }
        public List<TmTaskType> TaskTypes { get; set; }

        public IEnumerable<SelectListItem> ModuleSelectListItems
        {
            get
            {
                return new SelectList(Modules, "ModuleId", "ModuleName");
            }
        }

        public IEnumerable<SelectListItem> SubjectSelectListItems
        {
            get
            {
                return new SelectList(Subjects, "SubjectId", "SubjectName");
            }
        }

        public IEnumerable<SelectListItem> AssigneeSelectListItems
        {
            get
            {
                return new SelectList(Assignees, "AssigneeId", "Assignee");
            }
        }

        public IEnumerable<SelectListItem> TaskStatusSelectListItems
        {
            get
            {
                return new SelectList(TaskStatusList, "TaskStatusId", "TaskStatus");
            }
        }
        public IEnumerable<SelectListItem> TaskTypeSelectListItems
        {
            get
            {
                return new SelectList(TaskTypes, "TaskTypeId", "TaskType");
            }
        }
    }
}