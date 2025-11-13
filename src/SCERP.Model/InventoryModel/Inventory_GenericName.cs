using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;

namespace SCERP.Model.InventoryModel
{
    public class Inventory_GenericName
    {
        public int GenericNameId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string CompId { get; set; }
    }
}
