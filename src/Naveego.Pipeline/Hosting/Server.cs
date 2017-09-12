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
    internal class Server : IDisposable
    {
        private readonly Registry _registry;
        private readonly Action<string> _log;

        private NamedPipeServerStream _serverStream;

        public Server(Registry registry, Action<string> log = null)
        {
            _registry = registry;
            _log = HostUtils.DefaultLog("Server: ", log);
        }

        /// <summary>
        /// Starts the server at <paramref name="address"/> and runs it until 
        /// <paramref name="token"/> is cancelled.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task ListenAndServe(string address, CancellationToken token)
        {
            if (!address.StartsWith("namedpipe"))
            {
                throw new InvalidOperationException("Only named pipe transport is supported.");
            }

            var pipeName = address.Replace(@"namedpipes://\\.\pipe\", "");
            _log($"Listening and serving on '{pipeName}'");

            token.Register(() =>
            {
                _log($"Stop requested");
               if (_serverStream != null)
                {
                    if (_serverStream.IsConnected)
                    {
                        _serverStream.Disconnect();
                    }
                    _serverStream.Close();
                }
               
            });

            while (!token.IsCancellationRequested)
            { 
                _serverStream = new NamedPipeServerStream(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

                try
                {
                    await Task.Factory.FromAsync(_serverStream.BeginWaitForConnection, _serverStream.EndWaitForConnection, null);
                }
                catch (ObjectDisposedException)
                {
                    _log($"WaitForConnection aborted.");
                    return;
                }

                try
                {

                    _log($"Client connected.");
                   
                    Serve(_serverStream, _serverStream, token);
                }
                catch (IOException ex)
                {
                    _log($"Client disconnected: " +ex);
                    _serverStream?.Dispose();
                }
            }
        }
        

        public void Serve(Stream inputStream, Stream outputStream, CancellationToken token)
        {
            _log($"Serving....");

            using (var sr = new StreamReader(inputStream))
            using(var sw = new StreamWriter(outputStream))
            {
                sw.AutoFlush = true;

                var ser = JsonSerializer.Create(new JsonSerializerSettings
                {
                    CheckAdditionalContent = false,
                });
                while (!token.IsCancellationRequested)
                {
                    var reader = new JsonTextReader(sr);
                    var request = ser.Deserialize<JsonRequest>(reader);

                    var response = _registry.Handle(request);

                    var writer = new JsonTextWriter(sw);

                    ser.Serialize(writer, response);
                }
            }
        }

        public void Dispose()
        {
            _serverStream.Dispose();
        }
    }

    internal delegate JsonResponse JsonRpcHandler(JsonRequest request);

    [JsonObject()]
    internal class JsonRequest
    {
        public JsonRequest()
        {
        }

        [JsonProperty("jsonrpc")]
        public string JsonRpc => "1.0";

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params")]
        public object Params { get; set; }

        [JsonProperty("id")]
        public object Id { get; set; }
    }

    internal class JsonResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "jsonrpc")]
        public string JsonRpc =>"1.0"; 

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "result")]
        public object Result { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "error")]
        public object Error { get; set; }

        [JsonProperty(PropertyName = "id")]
        public object Id { get; set; }
    }

    /// <summary>
    ///  5.1 Error object
    ///
    ///  When a rpc call encounters an error, the Response Object MUST contain the error member with a value that is a Object with the following members:
    ///  codeA Number that indicates the error type that occurred.
    ///  This MUST be an integer.messageA String providing a short description of the error.
    ///  The message SHOULD be limited to a concise single sentence.dataA Primitive or Structured value that contains additional information about the error.
    ///  This may be omitted.
    ///  The value of this member is defined by the Server (e.g. detailed error information, nested errors etc.).
    ///  The error codes from and including -32768 to -32000 are reserved for pre-defined errors. Any code within this range, but not defined explicitly below is reserved for future use. The error codes are nearly the same as those suggested for XML-RPC at the following url: http://xmlrpc-epi.sourceforge.net/specs/rfc.fault_codes.php
    ///
    ///  code        message             meaning
    ///
    ///  -32700      Parse error         Invalid JSON was received by the server.  An error occurred on the server while parsing the JSON text.
    ///  -32600      Invalid Request     The JSON sent is not a valid Request object.
    ///  -32601      Method not found    The method does not exist / is not available.
    ///  -32602      Invalid params      Invalid method parameter(s).
    ///  -32603      Internal error      Internal JSON-RPC error.
    ///  -32000 to -32099 Server error   Reserved for implementation-defined server-errors.
    ///
    ///  The remainder of the space is available for application defined errors.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class JsonRpcException : System.ApplicationException
    {
        [JsonProperty]
        public int code { get; set; }

        [JsonProperty]
        public string message { get; set; }

        [JsonProperty]
        public object data { get; set; }

        public JsonRpcException(int code, string message, object data)
        {
            this.code = code;
            this.message = message;
            this.data = data;
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
