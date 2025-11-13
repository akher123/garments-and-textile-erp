using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SCERP.Model;

namespace SCERP.Web.Areas.Payroll.Models.ViewModels
{
    public class OvertimeSettingsViewModel: OvertimeSettings
    {
        public OvertimeSettingsViewModel()
        {
            OvertimeSettings=new List<OvertimeSettings>();
            
        }
        public DateTime? EndDate { get; set; }
        public List<OvertimeSettings> OvertimeSettings { get; set; }
    }
}