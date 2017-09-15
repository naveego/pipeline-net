using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.IO.Pipes;
using System.Net.Sockets;

namespace Naveego.Pipeline.Hosting.Clients
{
    public class TcpClientConnection : IClientConnection
    {
        private readonly TcpClient _client;
        private readonly StreamWriter _streamWriter;
        private readonly StreamReader _streamReader;


        public TcpClientConnection(string address, Action<string> log = null)
        {
            address = address.Replace("//:", "//127.0.0.1:");
            var url = new Uri(address);

            log = HostUtils.DefaultLog("TcpClient: ", log);

            log($"Connecting to {url} over TCP...");

            _client = new TcpClient(url.Host, url.Port);

            var stream = _client.GetStream();
            _streamReader = new StreamReader(stream);
            _streamWriter = new StreamWriter(stream)
            {
                AutoFlush = true
            };
            log("Connected.");
        }

        public StreamReader Input => _streamReader;

        public StreamWriter Output => _streamWriter;

        public void Dispose()
        {
            _client.Close();
        }
    }
}
