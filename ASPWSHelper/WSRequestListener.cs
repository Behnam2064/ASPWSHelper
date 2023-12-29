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
        private IList<WebSocket> clients { get; set; }

        public IReadOnlyCollection<WebSocket> Clients { get => new ReadOnlyCollection<WebSocket>(clients); } 

        public WSRequestListener()
        {
            clients = new List<WebSocket>();
            WebSocketOptions = new Microsoft.AspNetCore.Builder.WebSocketOptions()
            {
               KeepAliveInterval = TimeSpan.FromMinutes(2),// Default value is two minutes

            };
        }

        public async Task ListenAcceptAsync(object sender, HttpContext context)
        {
            WebSocket ws = await context.WebSockets.AcceptWebSocketAsync();
            clients.Add(ws);
            await ReceiveAsync(ws);
        }


        public abstract Task ReceiveAsync(WebSocket ws);
/*
        public async Task ReceiveAsync(WebSocket ws)
        {
            byte[] buff;
            while (ws.State == WebSocketState.Open)
            {
                buff = new byte[WSConfig.BufferSize];

                WebSocketReceiveResult result = await ws.ReceiveAsync(buff, CancellationToken.None);
                if (result != null)
                {
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string StrMsg = Encoding.UTF8.GetString(buff);
                        await SendAsync(ws, StrMsg + "***");
                    }
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        //your code 
                        return;
                    }
                }
            }
        }*/

        public async Task SendAsync(WebSocket ws, string Message)
        {
            byte[] buff = Encoding.UTF8.GetBytes(Message);
            await ws.SendAsync(buff, WebSocketMessageType.Text,true,CancellationToken.None);
        }

    }



}
