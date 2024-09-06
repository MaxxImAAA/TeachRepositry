using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Domain.Entity;

namespace Teach.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<IDbContextTransaction> BeginTransactionAsync();

        Task<int> SaveChangesAsync();

        IBaseRepository<User> Users { get; set; }

        IBaseRepository<Role> Roles { get; set; }

        IBaseRepository<UserRole> UserRoles { get; set; }
    }

    
}
