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
    /// Any input request that is sent as a WebSocket to the current interface
    /// </summary>
    public interface IWSRequestListener
    {
        public Encoding Encoding { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">Middleware</param>
        /// <param name="context"></param>
        Task ListenAcceptAsync(object sender, HttpContext context);

        Task ReceiveAsync(WebSocket ws, HttpContext context);

        Task SendAsync(WebSocket ws, string Message);

        public int GetBufferSize();

        public Task StopConnections();

        public Task<IReadOnlyList<T>> GetClients<T>();/* where T : class;*/

    }
}
