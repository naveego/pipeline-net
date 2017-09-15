using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Naveego.Pipeline.Hosting.Servers
{
    /// <summary>
    /// Server hosts a Registry at an Address. Currenttly 
    /// </summary>
    internal class TcpServer : Server
    {
        private Uri _addressUri;

        public TcpServer(string address, RequestHandler requestHandler, Action<string> log = null)
            : base(address, requestHandler, HostUtils.DefaultLog("TcpServer: ", log))
        {
            if (!address.StartsWith("tcp"))
            {
                throw new InvalidOperationException("Only named pipe transport is supported.");
            }

            _addressUri = new Uri(address);
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
            var listener = TcpListener.Create(_addressUri.Port);

            listener.Start();

            Log("Listener started.");

            try
            {
                while (!token.IsCancellationRequested)
                {

                    Socket socket = null;

                    try
                    {
                        socket = await listener.AcceptSocketAsync();
                        
                        Log($"Client connected.");

                        var cts = CancellationTokenSource.CreateLinkedTokenSource(token);

                        var stream = new CloseAwareNetworkStream(socket, cts);
                        
                        var writer = new StreamWriter(stream);
                        var reader = new StreamReader(stream);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        Task.Run(() =>
                        {
                            try
                            {
                                RequestHandler.Handle(reader, writer, cts.Token);
                            }
                            catch (IOException)
                            {
                                Log("Client disconnected.");
                            }
                            finally
                            {
                                socket.Close();
                            }
                        }, token);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    }

                    catch (SocketException ex)
                    {
                        Log($"Socket error: " + ex);
                    }
                }
            }
            finally
            {
                listener.Stop();
            }

        }

        public override void Dispose()
        {
        }
    }

    public class CloseAwareNetworkStream : NetworkStream
    {
        private readonly CancellationTokenSource cts;

        public CloseAwareNetworkStream(Socket socket, CancellationTokenSource cts) : base(socket)
        {
            this.cts = cts;
        }
        
        public override int Read(byte[] buffer, int offset, int size)
        {
            var amountRead = base.Read(buffer, offset, size);
            if(amountRead == 0)
            {
                // this means the socket has been closed; otherwise the socket would just block until it got some data
                cts.Cancel();
            }

            return amountRead;
        }
    }
}
