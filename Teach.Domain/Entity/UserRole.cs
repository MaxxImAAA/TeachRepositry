using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Domain.Interfaces;

namespace Teach.Domain.Entity
{
    public class UserRole : IEntityId<int>
    {
        public int Id { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
