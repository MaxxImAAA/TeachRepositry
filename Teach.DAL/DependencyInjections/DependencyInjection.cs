using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Teach.DAL.Interceptors;
using Teach.DAL.Repositories;
using Teach.Domain.Entity;
using Teach.Domain.Interfaces.Repositories;
using Teach.Domain.Result;

namespace Teach.DAL.DependencyInjections
{
    public static class DependencyInjection
    {
       
        public static void AddDataAccessLayer(this IServiceCollection service, 
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgresSQL");
           

            service.AddDbContext<ApplicationDbContext>(opt =>
            {
                 opt.UseNpgsql(connectionString);
               
            });

            service.AddSingleton<DateInterceptor>();

            service.InitRepositories();
            
           
        }

        private static void InitRepositories(this IServiceCollection service)
        {
            service.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
            service.AddScoped<IBaseRepository<Report>, BaseRepository<Report>>();
            service.AddScoped<IBaseRepository<UserToken>, BaseRepository<UserToken>>();
            service.AddScoped<IBaseRepository<Role>, BaseRepository<Role>>();
            service.AddScoped<IBaseRepository<UserRole>, BaseRepository<UserRole>>();
            service.AddScoped<IUnitOfWork, UnitOfWork>();

        }
    }
}
