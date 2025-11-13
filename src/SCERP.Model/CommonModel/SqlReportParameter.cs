using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;

namespace SCERP.Model.CommonModel
{
   public class SqlReportParameter
    {
        public long ReportParameterId { get; set; }
        public int CustomSqlQuaryId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string Pname { get; set; }
        public string Pvalue { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string LabelFor { get; set; }
        public string Querystring { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string ControlType { get; set; }
        public virtual CustomSqlQuary CustomSqlQuary { get; set; }
    }
}
