using System;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.Production
{
    public class ProSearchModel<T> : SearchModel<T> where T : class
    {
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }
        public virtual string SearchString { get; set; }
    }
}
