using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teach.Domain.Enum
{
    public enum ErrorCodes
    {
        ReportsNotFound = 0,
        ReportNotFound = 1,
        ReportAlreadyExists = 2,

        UserNotFound = 11,
        userPasswordIncorected = 21,
        UserIsRegistered = 22,

        PasswordIsWrong = 31,

        RoleNotFound = 32,

        InternalServerError = 10,

        InvalidClientrequest = 41,



        
    }
}
