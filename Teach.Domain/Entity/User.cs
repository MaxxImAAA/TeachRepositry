using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Domain.Interfaces;

namespace Teach.Domain.Entity
{
    public class User : IEntityId<long>
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }


        public List<Report> Reports { get; set; }

       public UserToken UserToken { get; set; }

        public List<UserRole> Roles { get; set; }
    }
}
