using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.IO.Pipes;

namespace Naveego.Pipeline.Hosting
{
    /// <summary>
    /// Contract for a type that can invoke methods over JSON-RPC.
    /// </summary>
    internal interface IClient : IDisposable
    {
        object Invoke(string methodName, object parameter, Type responseType);
    }

    /// <summary>
    /// Creates concrete implementations of IClient based on the provided address.
    /// </summary>
    internal class ClientFactory
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="serviceName"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public virtual IClient Create(string address, string serviceName, Action<string> log = null)
        {
            if (address.StartsWith("namedpipes://"))
            {
                return new NamedPipesClient(address, serviceName, log);
            }

            throw new ArgumentOutOfRangeException("address", address, "Scheme not supported.");
        }
    }

    /// <summary>
    /// IClient implementation that talks over NamedPipes.
    /// </summary>
    internal class NamedPipesClient : IClient
    {
        private readonly Action<string> _log;

        private readonly string _address;
        private readonly string _prefix;
        private readonly JsonSerializer _serializer;
        private int _idSeed = 0;
        private NamedPipeClientStream _clientStream;
        private StreamWriter _streamWriter;
        private StreamReader _streamReader;


        public NamedPipesClient(string address, string serviceName, Action<string> log = null)
        {
            _address = address.Replace(@"namedpipes://\\.\pipe\", "");

            _prefix = serviceName + ".";
            _serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                CheckAdditionalContent = false,
            });

            _log = HostUtils.DefaultLog("Host: ", log);
        }

        private void EnsureConnected()
        {
            if(_clientStream == null || !_clientStream.IsConnected)
            {
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
        }

        public object Invoke(string methodName, object parameter, Type responseType)
        {
            EnsureConnected();

            _log($"Invoking {methodName} with parameter {JsonConvert.SerializeObject(parameter)}");
            var request = new JsonRequest
            {
                Id = _idSeed++,
                Method = _prefix + methodName,
                Params = new object[] { parameter },
            };

            _log($"Request: {JsonConvert.SerializeObject(request)}");

            var writer = new JsonTextWriter(_streamWriter);
            _serializer.Serialize(writer, request);
            var reader = new JsonTextReader(_streamReader);
            var response = _serializer.Deserialize<JsonResponse>(reader);

            if (response.Error is JsonRpcException)
            {
                var responseError = response.Error as JsonRpcException;
                throw new JsonRpcException(responseError.code, responseError.message, responseError.data);
            }
            else if (response.Error != null)
            {
                throw new JsonRpcException(-1, response.Error.ToString(), response.Error);
            }

            var result = ((JObject)response.Result).ToObject(responseType);

            return result;
        }

        public void Dispose()
        {
            _streamReader?.Dispose();
            _streamWriter?.Dispose();
            _clientStream?.Close();
            _clientStream?.Dispose();
        }
    }

    internal static class ClientExtensions
    {
        public static TResult Invoke<TResult>(this IClient client, string methodName, object parameter)
            =>(TResult)client.Invoke(methodName, parameter, typeof(TResult));
        
    }
}
