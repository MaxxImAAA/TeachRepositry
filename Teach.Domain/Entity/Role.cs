using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Domain.Interfaces;

namespace Teach.Domain.Entity
{
    public class Role : IEntityId<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<UserRole> Users { get; set; }
    }
}
