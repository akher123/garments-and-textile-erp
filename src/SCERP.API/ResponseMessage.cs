using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCERP.API
{
    public class ResponseMessage<T> : ErrorMessage
    {
        public int Total { get; set; }
        public T Result { get; set; }
    }
}
