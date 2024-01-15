using ASPWSHelper.InterfaceServices;
using ASPWSHelper.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ASPWSHelper
{
    public abstract class WSRequestListener : IWSRequestListener
    {
        public static Microsoft.AspNetCore.Builder.WebSocketOptions WebSocketOptions;

        private static IList<WebSocket> clients { get; set; }

        /// <summary>
        /// All connected clients (WebSockets)
        /// </summary>
        public static IReadOnlyCollection<WebSocket> Clients { get => new ReadOnlyCollection<WebSocket>(clients); }

        public Encoding Encoding { get; set; }

        public WSRequestListener()
        {
            if (clients == null)
                clients = new List<WebSocket>();
            this.Encoding = Encoding.UTF8;
            WebSocketOptions = new Microsoft.AspNetCore.Builder.WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromMinutes(2),// Default value is two minutes

            };
        }

        public virtual async Task ListenAcceptAsync(object sender, HttpContext context)
        {
            //The following code is managed in Middleware
            //if (HttpContext.WebSockets.IsWebSocketRequest)

            WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
            clients.Add(ws);
            await ReceiveAsync(ws, context);
        }


        public abstract Task ReceiveAsync(WebSocket ws, HttpContext context);


        public virtual async Task SendAsync(WebSocket ws, string Message)
        {
            byte[] buff = this.Encoding.GetBytes(Message);
            await ws.SendAsync(buff, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public virtual int GetBufferSize()
         => WebSocketOptions.ReceiveBufferSize;

    }



}
