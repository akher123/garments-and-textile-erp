using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCERP.Model.HRMModel;

namespace SCERP.Web.Areas.HRM.Models.ViewModels
{
    public class SkillMatrixGradeViewModel : HrmSearchModel<SkillMatrixGradeViewModel>
    {
        public SkillMatrixGradeViewModel()
        {
          SkillMatrixGrade=new HrmSkillMatrixGrade();
          SkillMatrixGrades=new List<HrmSkillMatrixGrade>();
        }
        public HrmSkillMatrixGrade SkillMatrixGrade { get; set; }
        public List<HrmSkillMatrixGrade> SkillMatrixGrades { get; set; }
    }
}