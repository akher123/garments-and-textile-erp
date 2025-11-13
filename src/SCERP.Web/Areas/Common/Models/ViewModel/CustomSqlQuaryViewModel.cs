using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using SCERP.Model.CommonModel;

namespace SCERP.Web.Areas.Common.Models.ViewModel
{
    public class CustomSqlQuaryViewModel : CustomSqlQuary
    {
        public string Key { get; set; }
        public Dictionary<string, ReportParameterViewModel> ReportParameterViewModels { get; set; }
        public List<CustomSqlQuary> CustomSqlQuaries { get; set; }
        public List<VUserReport> VUserReports { get; set; }
        public DataTable DataTable { get; set; }
        public List<string> TableNameList { get; set; }
        public Dictionary<string,SqlReportParameter> Parameters { get; set; }
        public CustomSqlQuaryViewModel()
        {
            ReportParameterViewModels=new Dictionary<string, ReportParameterViewModel>();
            TableNameList=new List<string>();
            DataTable=new DataTable();
            CustomSqlQuaries=new List<CustomSqlQuary>();
            VUserReports=new List<VUserReport>();
            Parameters=new Dictionary<string, SqlReportParameter>();
        }

        public string TableName { get; set; }
        public IEnumerable<SelectListItem> TableNameSelectListItem
        {
            get
            {
                return new SelectList(TableNameList);
            }
        }
   

        public IEnumerable<object> ControlTypes
        {
            get
            {
                return new[]
                {
                     new{ControlType="text",Value="text"}
                    ,new{ControlType="date",Value="date"}
                    ,new{ControlType="dropdown",Value="dropdown"}
                };
            }
        }


    }
}