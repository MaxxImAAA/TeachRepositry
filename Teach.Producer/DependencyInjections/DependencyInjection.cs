using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Teach.Producer.Interfaces;

namespace Teach.Producer.DependencyInjections
{
    public static class DependencyInjection
    {
        public static void AddProducer(this IServiceCollection service)
        {
            service.AddScoped<IMessageProducer, Producer>();
        }
    }
}
