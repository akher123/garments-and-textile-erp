using System;


namespace SCERP.Model.Production
{
   
    
    public partial class VwReturnableChallanReceiveReport
    {
        public string Unit { get; set; }
        public double DeliveryQty { get; set; }
        public string Messrs { get; set; }
        public string Address { get; set; }
        public string RefferancePerson { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Phone { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ReceiveDate { get; set; }
        public double TotalAmount { get; set; }
        public double ReceiveQty { get; set; }
    }
}
