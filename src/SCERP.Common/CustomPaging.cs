using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common
{
    public static class CustomPaging
    {
        public static int GetSortDirection(string sortDirection)
        {

            switch (sortDirection)
            {
                case "ASC":
                    return 1;
                    break;
                case "DESC":
                    return 2;
                    break;
                default:
                    return 1;
                    break;
            }
        }
    }
}
