using Serilog;
using Teach.Application.DependencyInjections;
using Teach.DAL.DependencyInjections;
using Teach.Domain.Settings;
using Teach.WebApi;
using Teach.WebApi.Middlewares;
using Teach.Consumer.DependencyInjections;
using Teach.Producer.DependencyInjections;
var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection(nameof(RabbitMqSettings)));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.DefaultSection));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
/*builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
*/
builder.Services.AddAuthenticationAndAuthorization(builder);
builder.Services.AddSwagger();
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddApplication();
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddConsumer();
builder.Services.AddProducer();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Teach Swagger v 1.0");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "Teach Swagger v 2.0");
      //  c.RoutePrefix = string.Empty;
    });
}

app.AddExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
