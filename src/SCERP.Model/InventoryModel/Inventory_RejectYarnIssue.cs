using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Model.CommonModel;

namespace SCERP.Model.InventoryModel
{
    public class Inventory_RejectYarnIssue
    {
        public Inventory_RejectYarnIssue()
        {
            this.Inventory_RejectYarnIssueDetail = new HashSet<Inventory_RejectYarnIssueDetail>();
        }
       public int RejectYarnIssueId { get; set; }
       [Required]
       public string RefId { get; set; }
       [Required]
       public long PartyId { get; set; }
       [Required]
       public DateTime IssueDate { get; set; }
       public string ChallanNo { get; set; }
       public string IssueType { get; set; }
       public string Remarks { get; set; }
       public virtual Party Party { get; set; }
       public virtual ICollection<Inventory_RejectYarnIssueDetail> Inventory_RejectYarnIssueDetail { get; set; }

    }
}
