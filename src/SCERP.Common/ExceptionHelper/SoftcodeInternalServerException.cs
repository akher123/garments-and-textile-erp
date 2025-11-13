using System;
using System.Net;

namespace SCERP.Common.ExceptionHelper
{
    [Serializable]
    public class SoftcodeInternalServerException : SoftcodeException
    {
        public SoftcodeInternalServerException(string message)
            : base(message, (int)HttpStatusCode.InternalServerError)
        {
        }

        /// <summary>
        /// Optional Constructor that takes input for a String Format
        /// </summary>
        /// <param name="format">A string to format</param>
        /// <param name="args">input arguments for formatting the string</param>
        public SoftcodeInternalServerException(string format, params object[] args)
            : base(string.Format(format, args), (int)HttpStatusCode.InternalServerError)
        {
        }

    }
}
