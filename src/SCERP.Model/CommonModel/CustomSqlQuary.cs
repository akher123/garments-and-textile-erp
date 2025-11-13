using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SCERP.Common;

namespace SCERP.Model.CommonModel
{
    public partial  class CustomSqlQuary
    {
        public CustomSqlQuary()
        {
            SqlReportParameter=new HashSet<SqlReportParameter>();
        }
        public int CustomSqlQuaryId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string SqlQuaryRefId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string SqlQuary { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public Guid? EditedBy { get; set; }
        public bool IsActive { get; set; }
        public ICollection<SqlReportParameter> SqlReportParameter { get; set; }
    }
}
