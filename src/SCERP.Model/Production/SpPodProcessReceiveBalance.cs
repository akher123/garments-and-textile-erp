using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Production
{
    public class SpPodProcessReceiveBalance
    {
        public string BuyerName { get; set; }
        public string OrderNo { get; set; }
        public string StyleName { get; set; }
        public long CuttingTagId { get; set; }
        public long CuttingBatchId { get; set; }
        public string CuttingBatchRefId { get; set; }
        public string TagName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public int SendQuantity { get; set; }
        public int RecvQuantity { get; set; }
        public string ColorRefId { get; set; }
        public string SizeRefId { get; set; }
        public string JobNo { get; set; }
        public string Factory { get; set; }
    }
}
