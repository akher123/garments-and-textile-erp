using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.InventoryModel
{
    public class Inventory_GroupChallan
    {
        public Inventory_GroupChallan()
        {
            MaterialIssues = new List<Inventory_AdvanceMaterialIssue>();
        }
        public int GroupChallanId { get; set; }
        public string RefId { get; set; }
        public int GType { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public DateTime GDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? EditedDate { get; set; }
        public Guid? EditedBy { get; set; }
        public bool IsActive { get; set; }
        public string CompId { get; set; }
        public long PartyId { get; set; }
        [NotMapped]
        public List<Inventory_AdvanceMaterialIssue> MaterialIssues { get; set; }
    }
}
