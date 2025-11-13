using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.Common.ExceptionHelper
{
    [Serializable]
    public class SoftcodeDatabaseException : SoftcodeException
    {
        public SoftcodeDatabaseException(string message)
            : base(message, (int)HttpStatusCode.InternalServerError)
        {
        }

        public SoftcodeDatabaseException(string message, Exception innerException)
            : base(message, (int)HttpStatusCode.InternalServerError, innerException)
        {
        }

        public SoftcodeDatabaseException(string format, params object[] args)
            : base(string.Format(format, args), (int)HttpStatusCode.InternalServerError)
        {
        }

        public SoftcodeDatabaseException(Exception innerException, string format, params object[] args)
            : base(string.Format(format, args), (int)HttpStatusCode.InternalServerError, innerException)
        {
        }
    }
}
