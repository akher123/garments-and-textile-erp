using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model
{
    public class Contact
    {
        public Contact()
        {
            this.User = new HashSet<User>();
        }
        public Guid ContactId { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Guid? EmployeeId { get; set; }
        public string CompId { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
