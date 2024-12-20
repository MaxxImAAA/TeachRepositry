﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Domain.Interfaces;

namespace Teach.Domain.Entity
{
    public class UserToken : IEntityId<long>
    {
        public long Id { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }

        public User User { get; set; }

        public long UserId { get; set; }    
    }
}
