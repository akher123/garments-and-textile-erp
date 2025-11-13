using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;

namespace SCERP.Common
{
    public static class Errorlog
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Errorlog));

        public static void WriteLog(Exception filterContext)
        {
            log.Error(filterContext.Message + "\n" + Environment.StackTrace);
        }

    }
}
