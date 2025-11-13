using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SCERP.Web.Areas.MIS.Models.ViewModel
{
    public class CuttingStatusViewModel
    {
        public DataTable FabRcvDtl { get; set; }
        public DataTable BodyCuttDtl { get; set; }
        public DataTable RejectReplDtl { get; set; }
        public DataTable PrintDtl { get; set; }
        public DataTable EmbDtl { get; set; }
        public Dictionary<string, Dictionary<string, List<string>>> PivotDictionary { get; set; }
    }
}