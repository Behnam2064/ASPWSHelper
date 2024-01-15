using ASPWSHelper.InterfaceServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.IISIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPWSHelper.Middlewares
{
    public class WebSocketListenerMiddleware
    {
        private readonly RequestDelegate _next;

        public WebSocketListenerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context">Input request</param>
        /// <param name="listener">Receiving WebSockets requests</param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, IWSRequestListener listener)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                if (listener != null)
                    await listener.ListenAcceptAsync(this, context);
            }
            else
            {
                await _next(context);
            }
        }
    }


    public static class WSMiddlewareExtensions
    {
        public static IApplicationBuilder UseWSListenerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<WebSocketListenerMiddleware>();
        }
    }
}
