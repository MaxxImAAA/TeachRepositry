using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teach.Consumer.DependencyInjections
{
    public static class DependencyInjection
    {
        public static void AddConsumer(this IServiceCollection service)
        {
            service.AddHostedService<RabbitMqListener>();
        }
    }
}
