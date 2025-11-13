using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCERP.Model.HRMModel
{
    public class HrmSearchModel<T> : SearchModel<T> where T : class
    {
         public HrmSearchModel()
           {
               DataList = new List<T>();
           }
           [DataType(DataType.Date)]
           public DateTime? FromDate { get; set; }
           [DataType(DataType.Date)]
           public DateTime? ToDate { get; set; }

           public IEnumerable<T> DataList { get; set; }
           public virtual string SearchString { get; set; }
       }
    }
