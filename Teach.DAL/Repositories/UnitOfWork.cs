using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Domain.Entity;
using Teach.Domain.Interfaces.Repositories;

namespace Teach.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IBaseRepository<User> Users { get; set; }
        public IBaseRepository<Role> Roles { get; set; }
        public IBaseRepository<UserRole> UserRoles { get ; set; }

        public UnitOfWork(ApplicationDbContext _context, IBaseRepository<User> Users,
            IBaseRepository<Role> Roles, IBaseRepository<UserRole> UserRoles)
        {
            this._context = _context;
            this.Users = Users;
            this.Roles = Roles;
            this.UserRoles = UserRoles;
        }

        

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);

        }
    }
}
