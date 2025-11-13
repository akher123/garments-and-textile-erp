using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;

namespace SCERP.Model.InventoryModel
{
    public class SpInventoryStyleShipment
    {
        public string CompId { get; set; }
        public string SizeRefId { get; set; }
        public string ColorRefId { get; set; }
        public string SizeName { get; set; }
        public string ColorName { get; set; }
        public string OrderStyleRefId { get; set; }
        [Required(ErrorMessage = CustomErrorMessage.RequiredErrorMessage)]
        public int ShipmentQty { get; set; }
    }
}
