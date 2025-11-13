using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.AccountingModel
{
    public class Acc_PartyAccount
    {
        public int Id { get; set; }
        public long PartyId { get; set; }
        public int GLId { get; set; }
        public string PartyType { get; set; }
        public Guid Createdby { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EditedDate { get; set; }
        public Guid? EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
