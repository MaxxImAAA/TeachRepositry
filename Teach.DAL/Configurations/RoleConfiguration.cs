using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Domain.Entity;

namespace Teach.DAL.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(new List<Role>()
            {
                new Role()
                {
                    Id = 1,
                    Name = "Admin",
                    Users = new List<UserRole>()
                },

                new Role()
                {
                    Id = 2,
                    Name = "User",
                    Users = new List<UserRole>()
                },

                new Role()
                {
                    Id = 3,
                    Name = "Moderator",
                    Users = new List<UserRole>()
                },
            });
        }
    }
}
