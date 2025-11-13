using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common
{
    public static class GatepassType{
        public const string GeneralType="G";
        public const string YarnType = "Y";
        public const string YarnType_Receive = "1";
        public const string KnitFabType = "K";
        public const string SampleGatePass = "S";
    }

    public static class UniqueKey
    {
        private static string GenerateId()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

    }
}
