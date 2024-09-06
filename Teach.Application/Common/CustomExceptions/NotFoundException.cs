using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Application.Resources;

namespace Teach.Application.Common.CustomExceptions
{
    public class NotFoundException : Exception, ICustomException
    {
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }

        public NotFoundException(string ErrorMessage, int ErrorCode) : base()
        {
            this.ErrorMessage = ErrorMessage;
            this.ErrorCode = ErrorCode;
        }

        
    }
}
