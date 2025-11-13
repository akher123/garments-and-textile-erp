using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.Process.TimeAndAction
{
    public interface IActionSource
    {
        int SaveAction(Object actionObj);
        int EditAction(Object actionObj);
    }
}
