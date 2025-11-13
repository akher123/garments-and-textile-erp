using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common.ExceptionHelper
{
    [Serializable]
    public class SoftcodeException : Exception
    {
        public SoftcodeException(string message, int errorCode)
            : base(message)
        {
            HResult = errorCode;
        }

        public SoftcodeException(string message, int errorCode, Exception innerException)
            : base(message, innerException)
        {
            HResult = errorCode;
        }
    }
}
