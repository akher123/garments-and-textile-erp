using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common.ExceptionHelper
{
    [Serializable]
    public class SoftcodeNotFoundException : SoftcodeException
    {
        public SoftcodeNotFoundException(string message)
            : base(message, (int)HttpStatusCode.NotFound)
        {
        }

        public SoftcodeNotFoundException(string format, params object[] args)
            : this(string.Format(format, args))
        {
        }
    }
}
