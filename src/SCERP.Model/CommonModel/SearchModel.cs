using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCERP.Common;

namespace SCERP.Model
{
    public class SearchModel<T> where T : class
    {
      
        public int TotalRecords { get; set; }

        /// <summary>
        /// Sort column name
        /// </summary>
        public string sort { get; set; }

        /// <summary>
        /// Sort direction
        /// </summary>
        public string sortdir { get; set; }
        /// <summary>
        /// Set Report type excel,pdf
        /// </summary>
        public string ReportType { get; set; }


        public int? page { get; set; }
        public int PageIndex
        {
            get
            {
                int index = 0;
              //  int pageSize = AppConfig.PageSize;
                if (page.HasValue && page.Value > 0)
                {
                    index = page.Value - 1;
                }
                return index;
            }
        }
        public bool IsSearch { get; set; }
        public string DisplayMember { get; set; }
        public int ValueMember { get; set; }
        public virtual bool IsSelected { get; set; }

    }
}
