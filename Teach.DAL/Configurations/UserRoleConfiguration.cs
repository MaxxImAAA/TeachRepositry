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
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasOne(x=>x.User).WithMany(x=>x.Roles).HasForeignKey(x=>x.UserId);
            builder.HasOne(x => x.Role).WithMany(x => x.Users).HasForeignKey(x => x.RoleId);

            builder.HasData(new List<UserRole>()
            {
                new UserRole()
                {
                    Id = 1,
                    RoleId = 1,
                    UserId = 7
                }
            });
        }
    }
}
