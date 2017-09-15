using Naveego.Pipeline.Publishers;
using Naveego.Pipeline.Subscribers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Naveego.Pipeline.Hosting
{
    /// <summary>
    /// Host is the entrypoint to the hosting system. A process which wants
    /// to host a publisher or a subscriber should create an instance of Host
    /// and call WithSubscriber or WithPublisher, then await Run (optionally 
    /// passing in a CancellationToken, which will gracefully shut down the host 
    /// if cancelled). The Host instance should be disposed when no longer needed,
    /// which will tear down the connections more aggressively.
    /// </summary>
    public class Host : IDisposable
    {
        private readonly Action<string> _log;
        private readonly string _address;

        private Server _server;
        private ISubscriber _subscriber;
        private IPublisher _publisher;

        public Host(string address, Action<string> log = null)
        {
            _address = address;
            _log = HostUtils.DefaultLog("Host: ", log);
        }

        public Host WithSubscriber(ISubscriber subscriber)
        {
            _subscriber = subscriber;
            return this;
        }
        public Host WithPublisher(IPublisher publisher)
        {
            _publisher = publisher;
            return this;
        }

        public Task Run()
        {
            return Run(CancellationToken.None);
        }

        public Task Run(CancellationToken token)
        {
            if(_subscriber == null && _publisher == null)
            {
                throw new InvalidOperationException("You must call WithSubscriber or WithPublisher before running the host.");
            }
            var registry = new Registry(_log);

            var serverFactory = new ServerFactory();

            if(_subscriber != null)
            {
                var subscriber = new SubscriberAdapter(_subscriber, _log);
                registry.Register("Subscriber", subscriber);
            }

            if(_publisher != null)
            {
                var publisher = new PublisherAdapter(_publisher, new ClientFactory(), _log);
                registry.Register("Publisher", publisher);
            }

            _server = serverFactory.CreateServer(_address, registry, _log);

            return Task.Run(async () =>
            {
                await _server.ListenAndServe(token);
                _log("Stopped.");
            }, token);
        }

        public void Dispose()
        {
            ((IDisposable)_server)?.Dispose();
        }
    }
}
