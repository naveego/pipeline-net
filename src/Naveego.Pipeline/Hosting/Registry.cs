using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Naveego.Pipeline.Hosting
{
    /// <summary>
    /// Registry provides bindings between service instances and 
    /// JSON-RPC method names.
    /// </summary>
    internal class Registry
    {
        private readonly Action<string> _log;
        private readonly Dictionary<string, JsonRpcHandler> _handlers = new Dictionary<string, JsonRpcHandler>(StringComparer.OrdinalIgnoreCase);
        public IReadOnlyDictionary<string, JsonRpcHandler> Handlers => _handlers;

        public Registry(Action<string> log)
        {
            _log = HostUtils.DefaultLog("Registry", log);
        }

        /// <summary>
        /// Register a handler to be executed whenever we get a request for a method name.
        /// </summary>
        /// <param name="fullyQualifiedMethodName"></param>
        /// <param name="handler"></param>
        public void Register(string fullyQualifiedMethodName, JsonRpcHandler handler)
        {
            _handlers[fullyQualifiedMethodName] = WrapWithExceptionHandling(handler);
        }

        /// <summary>
        /// Register all methods on <paramref name="serviceInstance"/> which match 
        /// our JSON-RPC method convention (one parameter and one return value).
        /// The methods will be namespaced using <paramref name="serviceName"/>,
        /// so incoming requests should have method="serviceName.MethodName".
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceInstance"></param>
        public void Register(string serviceName, object serviceInstance)
        {

            var serviceType = serviceInstance.GetType();
            var methods = serviceType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                    .Where(m => m.GetParameters().Count() == 1
                                        && m.ReturnType != null);

            foreach(var method in methods)
            {
                var parameterType = method.GetParameters().First().ParameterType;

                JsonRpcHandler handler = (JsonRequest req) =>
                {
                    _log("Got request: " + JsonConvert.SerializeObject(req));


                    JToken jParameter = (req.Params as JArray)?.First;
                    if(jParameter == null)
                    {
                        return ErrorResponse(req.Id, -32602, "params must be an array with a single item or an object");
                    }

                    var param = jParameter.ToObject(parameterType);

                    var result = method.Invoke(serviceInstance, new[] { param });

                    var response = new JsonResponse
                    {
                        Id = req.Id,
                        Result = result
                    };

                    return response;
                };

                var methodName = serviceName + "." + method.Name;

                Register(methodName, handler);
            }
        }

        public JsonResponse Handle(JsonRequest request)
        {
            JsonRpcHandler handler;
            if(_handlers.TryGetValue(request.Method, out handler))
            {
                return handler(request);
            }
            return new JsonResponse
            {
                Id = request.Id,
                Error = new JsonRpcException(32601, "method not found", null),
            };
        }

        private JsonRpcHandler WrapWithExceptionHandling(JsonRpcHandler handler)
        {
            return (JsonRequest req) =>
            {
                try
                {
                    return handler(req);
                }
                catch (JsonRpcException ex)
                {
                    return new JsonResponse
                    {
                        Id = req.Id,
                        Error = ex
                    };
                }
                catch (Exception ex)
                {
                     _log($"Error from request {JsonConvert.SerializeObject(req)}: {ex}");
                   return new JsonResponse
                    {
                        Id = req.Id,
                        Error = new JsonRpcException(-1, ex.Message, null)
                    };
                }
            };
        }

        private JsonResponse ErrorResponse(object id, int code, string message)
        {
            return new JsonResponse()
            {
                Id = id,
                Error = new JsonRpcException(code, message, null)
            };
        }
    }
}
