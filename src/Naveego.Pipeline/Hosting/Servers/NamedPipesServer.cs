using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace Naveego.Pipeline.Hosting.Servers
{
    /// <summary>
    /// Server hosts a Registry at an Address. Currenttly 
    /// </summary>
    internal class NamedPipesServer : Server
    {
        private NamedPipeServerStream _serverStream;
        private string _address;

        public NamedPipesServer(string address, RequestHandler requestHandler, Action<string> log = null)
            : base(address, requestHandler, HostUtils.DefaultLog("NamedPipesServer: ", log))
        {
            if (!address.StartsWith("namedpipe"))
            {
                throw new InvalidOperationException("Only named pipe transport is supported.");
            }
            _address = address;
        }
        
        /// <summary>
        /// Starts the server at <paramref name="address"/> and runs it until 
        /// <paramref name="token"/> is cancelled.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public override async Task ListenAndServe(CancellationToken token)
        {
            var pipeName = _address.Replace(@"namedpipes://\\.\pipe\", "");
            Log($"Listening and serving on '{pipeName}'");

            token.Register(() =>
            {
                Log($"Stop requested");
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
                    Log($"WaitForConnection aborted.");
                    return;
                }

                try
                {

                    Log($"Client connected.");

                    var writer = new StreamWriter(_serverStream);
                    var reader = new StreamReader(_serverStream);

                    RequestHandler.Handle(reader, writer, token);
                }
                catch (IOException ex)
                {
                    Log($"Client disconnected: " + ex);
                }
                finally
                {
                    _serverStream?.Dispose();
                }
            }
        }
        
        public override void Dispose()
        {
            _serverStream?.Dispose();
        }
    }
}
