using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.HRMModel;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class SkillMatrixProcessViewModel : HrmSearchModel<SkillMatrixProcessViewModel>
    {
        public SkillMatrixProcessViewModel()
        {
            SkillMatrixProcess=new HrmSkillMatrixProcess();
            SkillMatrixProcesses=new List<HrmSkillMatrixProcess>();
        }
        public HrmSkillMatrixProcess SkillMatrixProcess { get; set; }
        public List<HrmSkillMatrixProcess> SkillMatrixProcesses { get; set; }

    }
}