using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
    public class PrintEmbProcessStatus
    {
        public string CompId { get; set; }
        public string OrderStyleRefId { get; set; }
        public int OrderQty { get; set; }
        public string ColorRefId { get; set; }
        public string ColorName { get; set; }
        public string SizeRefId { get; set; }
        public string SizeName { get; set; }
        public int SentQty { get; set; }
        public int InvQty { get; set; }
        public int FabReject { get; set; }
        public int ProcesReject { get; set; }
        public int SizeRow { get; set; }
    }
}
