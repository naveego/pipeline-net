using Naveego.Pipeline.Protocol;
using Naveego.Pipeline.Subscribers;
using System;

namespace Naveego.Pipeline.Hosting
{
    /// <summary>
    /// Adapter between published ISubscriber API and actual protocol.
    /// </summary>
    public class SubscriberAdapter
    {
        private readonly ISubscriber _subscriber;
        private readonly Action<string> log;

        public SubscriberAdapter(ISubscriber subscriber, Action<string> log)
        {
            _subscriber = subscriber;
            this.log = log;
        }

        public InitializeResponse Init(InitializeSubscriberRequest request)
            => _subscriber.Init(request);

        public TestConnectionResponse TestConnection(TestConnectionRequest request)
            => _subscriber.TestConnection(request);

        public DiscoverShapesResponse DiscoverShapes(DiscoverSubscriberShapesRequest request)
            => _subscriber.Shapes(request);

        public ReceiveDataPointResponse ReceiveDataPoint(ReceiveDataPointRequest request)
            => _subscriber.Receive(request);

        public DisposeResponse Dispose(DisposeRequest request)
        {
            _subscriber.Dispose(request);

            return new DisposeResponse();
        }
    }


}
