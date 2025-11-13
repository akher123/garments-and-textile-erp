using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public partial class SalaryHead:BaseModel
    {
        public SalaryHead()
        {
            this.SalarySetup = new HashSet<SalarySetup>();
            this.Acc_SalaryMapping = new HashSet<Acc_SalaryMapping>();
        }

        public int Id { get; set; }
        public string SalaryHeadName { get; set; }
        public string SalaryHeadDescription { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public virtual ICollection<SalarySetup> SalarySetup { get; set; }
        public virtual ICollection<Acc_SalaryMapping> Acc_SalaryMapping { get; set; }
    }
}
