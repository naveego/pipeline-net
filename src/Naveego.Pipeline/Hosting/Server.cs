using Naveego.Pipeline.Hosting.Servers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naveego.Pipeline.Hosting
{

    /// <summary>
    /// Server hosts a Registry at an Address. Currenttly 
    /// </summary>
    internal abstract class Server : IDisposable
    {
        protected readonly Action<string> Log;

        protected readonly RequestHandler RequestHandler;

        protected readonly string Address;

        protected Server(string address, RequestHandler requestHandler, Action<string> log = null)
        {
            Log = HostUtils.DefaultLog("Server: ", log);
            RequestHandler = requestHandler;
            Address = address;
        }

        /// <summary>
        /// Starts the server at <see cref="Address"/> and runs it until 
        /// <paramref name="token"/> is cancelled.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public abstract Task ListenAndServe(CancellationToken token);

        public virtual void Dispose() { }
    }

    internal class RequestHandler
    {
        private readonly Registry _registry;
        private readonly Action<string> _log;

        public RequestHandler(Registry registry, Action<string> log = null)
        {
            _registry = registry;
            _log = HostUtils.DefaultLog("Handler: ", log);
        }


        public void Handle(StreamReader input, StreamWriter output, CancellationToken token)
        {
            _log($"Serving....");
            output.AutoFlush = true;

            var ser = JsonSerializer.Create(new JsonSerializerSettings
            {
                CheckAdditionalContent = false,
            });
            while (!token.IsCancellationRequested)
            {
                var reader = new JsonTextReader(input);
                var request = ser.Deserialize<JsonRequest>(reader);
                if (request != null)
                {
                    var response = _registry.Handle(request);

                    var writer = new JsonTextWriter(output);

                    ser.Serialize(writer, response);

                }

            }
        }
    }

    internal class ServerFactory
    {
        public Server CreateServer(string address, Registry registry, Action<string> log)
        {
            if (address == null) throw new ArgumentNullException("address");

            var handler = new RequestHandler(registry, log);

            if (address.StartsWith("namedpipe"))
            {
                return new NamedPipesServer(address, handler, log);
            }

            if (address.StartsWith("tcp"))
            {
                return new TcpServer(address, handler, log);
            }

            throw new ArgumentOutOfRangeException("address", address, "Protocol is not supported.");
        }
    }



    internal static class HostUtils
    {
        public static Action<string> DefaultLog(string prefix, Action<string> log)
        {
            if (log != null)
            {
                return s => log($"{prefix} {s}");
            }
            else
            {
                return s => { };
            }
        }
    }
}
