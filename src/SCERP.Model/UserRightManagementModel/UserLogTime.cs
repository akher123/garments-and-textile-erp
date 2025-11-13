using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.UserRightManagementModel
{
   public partial  class UserLogTime
    {
        public Guid UserLogTimeId { get; set; }
        public Guid UserId { get; set; }
        public string SessionId { get; set; }
        public string UserHostAddress { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVerssion { get; set; }
        public bool Offline { get; set; }
        
    }

}
