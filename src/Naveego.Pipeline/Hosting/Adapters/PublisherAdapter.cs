using Naveego.Pipeline;
using Naveego.Pipeline.Protocol;
using Naveego.Pipeline.Publishers;
using Naveego.Pipeline.Logging;
using System;

namespace Naveego.Pipeline.Hosting
{
    /// <summary>
    /// Adapter between published IPublisher API and actual protocol.
    /// </summary>
    internal class PublisherAdapter
    {
        private readonly IPublisher _publisher;
        private readonly ClientFactory _clientFactory;
        private readonly Action<string> log;

        public PublisherAdapter(IPublisher publisher, ClientFactory clientFactory, Action<string> log)
        {
            _publisher = publisher;
            _clientFactory = clientFactory;
            this.log = log;
        }

        public InitializeResponse Init(InitializePublisherRequest request)
        { 
            if(_publisher is AbstractPublisher)
            {
                (_publisher as AbstractPublisher).Logger = new CombinedLogger(
                    new FileLogger(),
                    new DataFlowLogger(
                        request.Meta.GetValueIfPresent("tenant"),
                        request.Meta.GetValueIfPresent("logUrl")));
            }
            return _publisher.Init(request);
        }

        public TestConnectionResponse TestConnection(TestConnectionRequest request)
            => _publisher.TestConnection(request);

        public DiscoverShapesResponse DiscoverShapes(DiscoverPublisherShapesRequest request)
            => _publisher.Shapes(request);

        public PublishResponse Publish(PublishRequest request)
        {
            var client = _clientFactory.Create(request.PublishToAddress, "PublisherClient", this.log);
            var transport = new DataTransportAdapter(client);
            return _publisher.Publish(request, transport);
        }

        public DisposeResponse Dispose(DisposeRequest request)
        {
            _publisher.Dispose(request);

            return new DisposeResponse() { Success = true };
        }
    }


}
