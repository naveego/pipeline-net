using Naveego.Pipeline.Hosting.Clients;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

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
            var connection = CreateClientConnection(address, log);

            return new Client(connection, serviceName, log);
        }

        private IClientConnection CreateClientConnection(string address, Action<string> log)
        {
            if (address.StartsWith("namedpipe"))
            {
                return new NamedPipesClientConnection(address, log);
            }

            if (address.StartsWith("tcp"))
            {
                return new TcpClientConnection(address, log);
            }

            throw new ArgumentOutOfRangeException("address", address, "Scheme not supported.");            
        }
    }

    /// <summary>
    /// IClient implementation that talks over TCP.
    /// </summary>
    internal class Client : IClient
    {
        private readonly Action<string> _log;
        private readonly string _prefix;
        private readonly JsonSerializer _serializer;
        private int _idSeed = 0;

        private IClientConnection _connection;

        public Client(IClientConnection connection, string serviceName, Action<string> log = null)
        {
            _connection = connection;

            _prefix = serviceName + ".";
            _serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                CheckAdditionalContent = false,
            });

            _log = HostUtils.DefaultLog("Client: ", log);
        }

        public object Invoke(string methodName, object parameter, Type responseType)
        {
            _log($"Invoking {methodName} with parameter {JsonConvert.SerializeObject(parameter)}");
            var request = new JsonRequest
            {
                Id = _idSeed++,
                Method = _prefix + methodName,
                Params = new object[] { parameter },
            };

            _log($"Request: {JsonConvert.SerializeObject(request)}");

            var writer = new JsonTextWriter(_connection.Output);
            _serializer.Serialize(writer, request);
            var reader = new JsonTextReader(_connection.Input);
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

            _log($"Response: {response.Result}");

            return result;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }


    internal interface IClientConnection : IDisposable
    {
        StreamReader Input { get; }
        StreamWriter Output { get; }
    }

    internal static class ClientExtensions
    {
        public static TResult Invoke<TResult>(this IClient client, string methodName, object parameter)
            =>(TResult)client.Invoke(methodName, parameter, typeof(TResult));
        
    }
}
