using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teach.Application.Common.CustomExceptions
{
    public interface ICustomException
    {
        string ErrorMessage { get; set; }
        int ErrorCode { get; set; } 
    }
}
