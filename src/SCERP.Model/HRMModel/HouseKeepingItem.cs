using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.HRMModel
{
    public class HouseKeepingItem
    {
        public HouseKeepingItem()
        {
            this.HouseKeepingRegister = new HashSet<HouseKeepingRegister>();
        }
        public int HouseKeepingItemId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string HkItemRefId { get; set; }
        public string Description { get; set; }
        [Required]
        public string CompId { get; set; }
        public virtual ICollection<HouseKeepingRegister> HouseKeepingRegister { get; set; }
    }
}
