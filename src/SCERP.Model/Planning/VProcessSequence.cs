using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Model.Planning
{
   public class VProcessSequence
    {
        public long ProcessSequenceId { get; set; }
        public string CompId { get; set; }
        public string OrderStyleRefId { get; set; }
        public int ProcessRow { get; set; }
        public string ProcessRefId { get; set; }
        public string ProcessName { get; set; }
    }
}
