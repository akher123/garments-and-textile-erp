using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.AccountingModel
{
    public class Acc_GLAccounts_Hidden_Status : BaseModel
    {
        public Acc_GLAccounts_Hidden_Status()
        {

        }
        public int Id { get; set; }
        public bool Status { get; set; }
    }
}