using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ASPWSHelper.InterfaceServices
{
    /// <summary>
    /// Must be registered as Singleton
    /// Any input request that is sent as a WebSocket to the current interface
    /// </summary>
    public interface IWSRequestListener
    {
        public Encoding Encoding { get; set; }

        /// <summary>
        /// This method is most likely sent from a middleware
        /// </summary>
        /// <param name="sender">Middleware</param>
        /// <param name="context"></param>
        Task ListenAcceptAsync(object sender, HttpContext context);

        Task ReceiveAsync(WebSocket ws, HttpContext context);

        Task SendAsync(WebSocket ws, string Message);

        public int GetBufferSize();

    }
}
