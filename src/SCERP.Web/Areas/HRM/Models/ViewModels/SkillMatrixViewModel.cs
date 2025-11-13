using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCERP.Model;
using SCERP.Model.HRMModel;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class SkillMatrixViewModel : HrmSearchModel<SkillMatrixViewModel>
    {
        [Required]
        public string EmployeeCardId { get; set; }
        public string Key { get; set; }
        public SkillMatrixViewModel()
        {
          SkillMatrix=new HrmSkillMatrix();
          SkillMatrixProcess=new HrmSkillMatrixProcess();
          SkillMatrixDetail=new HrmSkillMatrixDetail();
          VwSkillMatrix=new VwSkillMatrix();
          SkillMatrixProcesses = new List<HrmSkillMatrixProcess>();
          VwSkillMatrixEmployeeList = new List<VwSkillMatrixEmployee>();
          VwSkillMatrices=new List<VwSkillMatrix>();
          EmployeeCompanyInfo=new VEmployeeCompanyInfoDetail();
          SkillMatrixDictionary=new Dictionary<string, VwSkillMatrix>();
        }
        public HrmSkillMatrix SkillMatrix { get; set; }
        public HrmSkillMatrixProcess SkillMatrixProcess { get; set; }
        public HrmSkillMatrixDetail SkillMatrixDetail { get; set; }
        public VwSkillMatrix VwSkillMatrix { get; set; }
        public List<VwSkillMatrix> VwSkillMatrices { get; set; }
        public VEmployeeCompanyInfoDetail EmployeeCompanyInfo { get; set; }
        public Dictionary<string, VwSkillMatrix> SkillMatrixDictionary { get; set; }
        public List<VwSkillMatrixEmployee> VwSkillMatrixEmployeeList { get; set; } 
        public List<HrmSkillMatrixProcess> SkillMatrixProcesses { get; set; }
        public IEnumerable<SelectListItem> SkillMatrixProcessesSelectListItem
        {
            get
            {
                return new SelectList(SkillMatrixProcesses, "SkillMatrixProcessId", "ProcessName");
            }
        }

    }
}