using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teach.Application.Common.CustomExceptions
{
    public class CustomBaseException : Exception, ICustomException
    {
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }

        public CustomBaseException(string ErrorMessage, int ErrorCode) : base()
        {
            this.ErrorMessage = $"Ошибка: {ErrorMessage}";
            this.ErrorCode = ErrorCode;
        }

        public override string ToString()
        {
            return $"Ошибка: {ErrorMessage}";
        }


    }
}
