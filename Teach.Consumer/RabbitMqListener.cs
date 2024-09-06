using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teach.Domain.Settings;

namespace Teach.Consumer
{
   
    public class RabbitMqListener : BackgroundService
    { 
        private readonly IConnection _connection;
        private readonly IModel _chanel;
        private readonly IOptions<RabbitMqSettings> _options;

        public RabbitMqListener(IOptions<RabbitMqSettings> _options)
        {
           
            this._options = _options;

            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _chanel = _connection.CreateModel();
            _chanel.QueueDeclare(_options.Value.QueueName, durable:true, exclusive:false, autoDelete:false, arguments:null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_chanel);
            consumer.Received += (obj, basicDeliver) =>
            {
                var content = Encoding.UTF8.GetString(basicDeliver.Body.ToArray());
                Debug.WriteLine($"Получено сообщение: {content}");

                _chanel.BasicAck(basicDeliver.DeliveryTag, false);
            };

            _chanel.BasicConsume(_options.Value.QueueName,autoAck:false, consumer);

            //Dispose();

            return Task.CompletedTask;
            
        }

        public override void Dispose()
        {
            _chanel.Dispose();
            _connection.Dispose();
            base.Dispose();
        }





    }

    
}
