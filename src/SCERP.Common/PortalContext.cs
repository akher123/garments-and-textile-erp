using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common
{
    public class PortalContext
    {
        public static PortalUser CurrentUser
        {
            get { var value = CommonSession.GetValue(SessionKey.PortalUser); return value != null ? (PortalUser)value : new PortalUser(); }
            set { CommonSession.SetValue(SessionKey.PortalUser, value); }
        }
    }
}
