using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common
{
    public class FunctionKeyStatus : DisplayProperty
    {
        public byte FunctionKey { get; set; }
    }

    public class IsFromMachineStatus : DisplayProperty
    {
        public bool IsFromMachineKey { get; set; }
    }


    public  static class AttendanceConfiguration
    {
        public static string DisplayFunctionKeyStatus(int functionKey)
        {
            var status = GetFunctionKeyStatusStatus().Find(x => x.FunctionKey == functionKey).Text;
            return status;

        }
        public static string DisplayIsFromMachineStatus(bool isFromMachine)
        {
            var status = GetIsFromMachineKeyStatusStatus().Find(x => x.IsFromMachineKey == isFromMachine).Text;
            return status;
        }

        public static List<IsFromMachineStatus> GetIsFromMachineKeyStatusStatus()
        {
            return new List<IsFromMachineStatus>
            {
                new IsFromMachineStatus()
                {
                    IsFromMachineKey = true,
                    Text = "Automatic"
                },
                 new IsFromMachineStatus()
                {
                    IsFromMachineKey = false,
                    Text = "Manual"
                },

            };
        }
        public static List<FunctionKeyStatus> GetFunctionKeyStatusStatus()
        {
            return new List<FunctionKeyStatus>
            {
                new FunctionKeyStatus()
                {
                    FunctionKey = 99,
                    Text = "Out"
                },
                 new FunctionKeyStatus()
                {
                    FunctionKey = 0,
                    Text = "In"
                },

            };
        }
    }
}
