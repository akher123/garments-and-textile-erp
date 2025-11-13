using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.Process.TimeAndAction
{
    public class TimeAndCalendarAction
    {
        IActionSource currentActionSource = null;

        public IActionSource ActionSource
        {
            get
            {
                return currentActionSource;
            }
            set
            {
                currentActionSource = value;
            }
        }

        public int SaveAction(object actionObj)
        {
            int save = 0;
            if (currentActionSource != null)
            {
                save = currentActionSource.SaveAction(actionObj);
            }
            return save;
        }

        public int EditAction(object actionObj)
        {
            int save = 0;
            if (currentActionSource != null)
            {
                save = currentActionSource.EditAction(actionObj);
            }
            return save;
        }
    }
}
