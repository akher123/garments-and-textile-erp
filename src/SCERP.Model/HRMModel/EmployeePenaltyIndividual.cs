using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
   public class EmployeePenaltyIndividual
    {
        public string EmployeeCardId { get; set; }
        public int Year { get; set; }
        public decimal PenaltyAmount { get; set; }
        public string Type { get; set; }
    }
}