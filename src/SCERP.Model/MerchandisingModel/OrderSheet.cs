using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace SCERP.Model.MerchandisingModel
{
    public class OrderStyle
    {
        public VOMBuyOrdStyle BuyOrdStyle { get; set; }
        public DataTable OrderShipTable { get; set; }
        public List<Shipment> Shipments { get; set; }
        public OrderStyle()
        {
            BuyOrdStyle=new VOMBuyOrdStyle();
            OrderShipTable=new DataTable();
            Shipments=new List<Shipment>();
        }
    }

    public class OrderSheet
    {
        public VBuyerOrder VBuyerOrder { get; set; }
        public Dictionary<string, OrderStyle> OrderStyles { get; set; }
        public OrderSheet()
        {
            VBuyerOrder=new VBuyerOrder();
            OrderStyles=new Dictionary<string, OrderStyle>();
        }
    }

    public class Shipment
    {
        public Shipment()
        {
            this.OrdShip=new VwOM_BuyOrdShip();
        }
        public VwOM_BuyOrdShip OrdShip { get; set; }
        public DataTable ShipTable { get; set; }
    }
}
