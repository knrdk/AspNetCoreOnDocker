using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using NLog;

namespace AspNetCoreOnDocker.Api.Middlewares
{
    public static class CorrelationContextMiddleware
    {
        private const string CORRELATION_ID_HEADER = "X-CorrelationId";
        private const string CORRELATION_ID_LOGS_NAME = "CorrelationId";

        public static void UseCorrelationContext(this IApplicationBuilder app)
        {
            app.Use(ConfigureCorrelationContext);            
        }

        private static async Task ConfigureCorrelationContext(Microsoft.AspNetCore.Http.HttpContext context, Func<Task> next)
        {
            string correlationId = GetCorrelationId(context);
            SetCorrelationIdInLogContext(correlationId);
            SetCorrelationIdInResponseHeader(context, correlationId);

            await next.Invoke();
        }

        private static string GetCorrelationId(Microsoft.AspNetCore.Http.HttpContext context)
        {
            string correlationId;
            if (context.Request.Headers.ContainsKey(CORRELATION_ID_HEADER))
            {
                correlationId = context.Request.Headers[CORRELATION_ID_HEADER];
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
            }

            return correlationId;
        }

        private static void SetCorrelationIdInLogContext(string correlationId)
        {
            MappedDiagnosticsLogicalContext.Set(CORRELATION_ID_LOGS_NAME, correlationId);
        }

        private static void SetCorrelationIdInResponseHeader(Microsoft.AspNetCore.Http.HttpContext context, string correlationId)
        {
            context.Response.Headers.Add(CORRELATION_ID_HEADER, correlationId);
        }
    }
}