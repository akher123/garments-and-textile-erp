using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Maintenance
{
    public class MaintenanceSearchModel<T> : SearchModel<T> where T : class
    {
        public MaintenanceSearchModel()
        {
            DataList=new List<T>();
        }
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        public IEnumerable<T> DataList { get; set; }
        public virtual string SearchString { get; set; }
    }
}
