using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.HRMModel;
using System.Web.Mvc;
using SCERP.Model.Custom;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class SkillMatrixNew
    {
        public SkillMatrixNew()
        {
            SkillMatrixDetails = new List<SkillMatrixDetailProcess>();
        }
        public List<SkillMatrixDetailProcess> SkillMatrixDetails { get; set; }

        public int MachineTypeId { get; set; }
        public List<SkillMatrixMachineType> SkillMatrixMachineTypes { get; set; }
        public IEnumerable<SelectListItem> SkillMatrixMachineTypesSelectListItem
        {
            get
            {
                return new SelectList(SkillMatrixMachineTypes, "MachineTypeId", "MachineTypeName");
            }
        }
        public SkillMatrixPointTable SkillMatrixPointTable { get; set; }



        public int SkillMatrixId { get; set; }
        public System.Guid EmployeeId { get; set; }
        public string EmployeeCardId { get; set; }
        public Nullable<double> Performance { get; set; }
        public Nullable<double> Attitude { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<System.Guid> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}