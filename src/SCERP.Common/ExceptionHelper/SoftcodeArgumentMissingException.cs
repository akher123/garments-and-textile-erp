using System;
using System.Net;

namespace SCERP.Common.ExceptionHelper
{
    [Serializable]
    public class SoftcodeArgumentMissingException : SoftcodeException
    {
        public SoftcodeArgumentMissingException(string message)
            : base(message, (int)HttpStatusCode.BadRequest)
        {
        }

        public SoftcodeArgumentMissingException(string format, params object[] args)
            : this(string.Format(format, args))
        {
        }
    }
}
