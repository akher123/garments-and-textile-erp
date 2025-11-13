using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.CommonModel
{
    public class VwParty
    {
    
        public string Name { get; set; }
        public string PartyRefNo { get; set; }
        public long PartyId { get; set; }
        public string CompId { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPhone { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public Guid? EditedBy { get; set; }
        public bool IsActive { get; set; }
        public string PType { get; set; }
        public int? DglId { get; set; }
        public string DyeingAcName { get; set; }
        public int? KglId { get; set; }
        public int? KRglId { get; set; }
        public string KnittingAcName { get; set; }
        public string KnittingRcvAcName { get; set; }
        public int? PGlId { get; set; }
        public int? EmGlId { get; set; }
        public string PrintPayAcName { get; set; }
        public string EmPayAcName { get; set; }

    }
}
