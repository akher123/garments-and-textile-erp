using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.TrackingModel
{
    public class TrackingSearchModel<T> : SearchModel<T> where T : class
    {
        public TrackingSearchModel()
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
