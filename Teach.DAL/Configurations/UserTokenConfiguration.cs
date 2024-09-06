﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Domain.Entity;

namespace Teach.DAL.Configurations
{
    public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.RefreshToken).IsRequired();
            builder.Property(x => x.RefreshTokenExpiryTime).IsRequired();

            builder.HasData(new List<UserToken>()
            {
                new UserToken()
                {
                    Id = 1,
                    RefreshToken = "kjnsdnnfkjKJAF",
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7),
                    UserId = 1
                }
            });
        }
    }
}
