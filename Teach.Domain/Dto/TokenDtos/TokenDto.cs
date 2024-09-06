using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teach.Domain.Dto.TokenDtos
{
    public class TokenDto
    {
        public string AccesToken { get; set; }
        
        public string RefreshToken { get;set; }
    }
}
