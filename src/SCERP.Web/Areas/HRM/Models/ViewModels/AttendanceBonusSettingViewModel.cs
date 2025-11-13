using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{

    public class AttendanceBonusSettingViewModel : AttendanceBonusSetting
    {
        public AttendanceBonusSettingViewModel()
        {
            AttendanceBonusSettings = new List<AttendanceBonusSetting>();
            IsSearch = true;
        }

        public List<AttendanceBonusSetting> AttendanceBonusSettings { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Required !")]
        public Nullable<System.DateTime> SearchByFromDate { get; set; }

        [DataType(DataType.Date)]

        public Nullable<System.DateTime> SearchByToDate { get; set; }

    }
}