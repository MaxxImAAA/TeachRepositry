using Npgsql;
using Serilog;
using System.Text.Json;
using Teach.Application.Common.CustomExceptions;
using Teach.Domain.Enum;
using Teach.Domain.Result;

namespace Teach.WebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;

        public ExceptionMiddleware(RequestDelegate _next, Serilog.ILogger _logger)
        {
            this._next = _next;
            this._logger = _logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.Error(ex, ex.Message);

            BaseResult baseResult = null;

            switch (ex)
            {

                case CustomBaseException cust:
                    {
                        baseResult = new BaseResult
                        {
                            //ErrorMessage = JsonSerializer.Serialize(cust.ToString()),
                            ErrorMessage = cust.ErrorMessage,
                            ErrorCode = cust.ErrorCode,
                        };
                        break;
                    }

            }
            
            if(baseResult == null)
            {
               
                baseResult = new BaseResult
                {
                    ErrorMessage = ex.Message,
                    ErrorCode = (int)ErrorCodes.InternalServerError
                };
               
            }

            context.Response.ContentType = "application/json";
            //context.Response.StatusCode = (int)baseResult.ErrorCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(baseResult));
            
        }
    }
}
