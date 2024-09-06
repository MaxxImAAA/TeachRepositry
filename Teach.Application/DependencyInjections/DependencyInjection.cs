using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Teach.Application.Services;
using Teach.Application.Validations;
using Teach.Application.Validations.FluentValidation.Report;
using Teach.Domain.Dto.Report;
using Teach.Domain.Interfaces.Services;
using Teach.Domain.Interfaces.Validations;

namespace Teach.Application.DependencyInjections
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services) 
        {

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            InitServices(services);
        }

        private static void InitServices(this IServiceCollection services)
        {
            services.AddScoped<IReportValidator, ReportValidator>();

            services.AddScoped<IValidator<CreateReportDto>, CreateReportValidator>();
            services.AddScoped<IValidator<UpdateReportDto>, UpdateReportValidator>();

            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRoleService, RoleService>();
        }
    }
}
