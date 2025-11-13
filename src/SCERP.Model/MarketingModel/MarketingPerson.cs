using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.MarketingModel
{
    public class MarketingPerson
    {
        public MarketingPerson()
        {
            MarketingInquiry = new HashSet<MarketingInquiry>();
        }
        public int MarketingPersonId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? QuitDate { get; set; }
        public bool IsActive { get; set; }
        public HashSet<MarketingInquiry> MarketingInquiry { get; set; }
    }
}
