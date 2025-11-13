using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public class Acc_BankReconcilationList
    {
        public int Id { get; set; }
        public string Sector { get; set; }
        public string FinancialPeriod { get; set; }
        public string AccountName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int NoOfReconciled { get; set; }
        public int NoOfPending { get; set; }
    }
}
