using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.BLL.IManager.ICommonManager
{
    public interface ISmsManager
    {
        bool Send(string mobileNumber, string message);

        bool Send(List<string> mobileNumbers, string message);

    }
}
