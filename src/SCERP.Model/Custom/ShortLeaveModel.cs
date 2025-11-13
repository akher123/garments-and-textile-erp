using System.ComponentModel.DataAnnotations;
using SCERP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SCERP.Model.Custom
{
    public class ShortLeaveModel : EmployeeShortLeave
    {
        public ShortLeaveModel()
        {
            //employeeShortLeaves = new List<EmployeeShortLeave>();

            VEmployeeShortLeave = new List<VEmployeeShortLeave>();
            IsSearch = true;
        }

        public List<VEmployeeShortLeave> VEmployeeShortLeave { get; set; }

        //public List<EmployeeShortLeave> employeeShortLeaves { get; set; }

        [Required(ErrorMessage = @"Required")]
        public string EmployeeCardId { get; set; }

        [Required(ErrorMessage = @"Required")]
        public DateTime? fromDate { get; set; }

        [Required(ErrorMessage = @"Required")]
        public DateTime? toDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = @"Required")]
        public DateTime? Date { get; set; }

        [Required(ErrorMessage = @"Required")]
        public string FromHourKey { get; set; }

        [Required(ErrorMessage = @"Required")]
        public string FromMinuteKey { get; set; }

        [Required(ErrorMessage = @"Required")]
        public string FromPeriodKey { get; set; }

        [Required(ErrorMessage = @"Required")]
        public string ToHourKey { get; set; }

        [Required(ErrorMessage = @"Required")]
        public string ToMinuteKey { get; set; }

        [Required(ErrorMessage = @"Required")]
        public string ToPeriodKey { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> FromHourSelectList
        {
            get { return new SelectList(TimeConfiguration.GetHours(), "HourKey", "Text"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> FromMinuteSelectList
        {
            get { return new SelectList(TimeConfiguration.GetMunites(), "MinuteKey", "Text"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> FromPeriodSelectList
        {
            get { return new SelectList(TimeConfiguration.GePeriods(), "PeriodKey", "Text"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> ToHourSelectList
        {
            get { return new SelectList(TimeConfiguration.GetHours(), "HourKey", "Text"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> ToMinuteSelectList
        {
            get { return new SelectList(TimeConfiguration.GetMunites(), "MinuteKey", "Text"); }
        }

        public IEnumerable<System.Web.Mvc.SelectListItem> ToPeriodSelectList
        {
            get { return new SelectList(TimeConfiguration.GePeriods(), "PeriodKey", "Text"); }
        }
    }
}
