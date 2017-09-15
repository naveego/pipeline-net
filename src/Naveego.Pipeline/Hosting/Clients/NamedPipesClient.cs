using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.IO.Pipes;

namespace Naveego.Pipeline.Hosting.Clients
{
    /// <summary>
    /// IClient implementation that talks over NamedPipes.
    /// </summary>
    internal class NamedPipesClientConnection : IClientConnection
    {
        private readonly Action<string> _log;

        private readonly string _address;
        private NamedPipeClientStream _clientStream;
        private readonly StreamWriter _streamWriter;
        private readonly StreamReader _streamReader;


        public NamedPipesClientConnection(string address, Action<string> log = null)
        {
            _address = address.Replace(@"namedpipes://\\.\pipe\", "");
             _log = HostUtils.DefaultLog("Host: ", log);
            _log($"Connecting to named pipe {_address}...");
            _clientStream = new NamedPipeClientStream(_address);
            _clientStream.Connect();
            _streamReader = new StreamReader(_clientStream);
            _streamWriter = new StreamWriter(_clientStream)
            {
                AutoFlush = true
            };
            _log("Connected.");
        }

        public StreamReader Input => _streamReader;

        public StreamWriter Output => _streamWriter;

        public void Dispose()
        {
            _streamReader?.Dispose();
            _streamWriter?.Dispose();
            _clientStream?.Close();
            _clientStream?.Dispose();
        }
    }
}
