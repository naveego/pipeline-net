using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naveego.Pipeline.Protocol;

namespace Naveego.Pipeline.Subscribers
{
    public interface ISubscriber
    {

        InitializeResponse Init(InitializeSubscriberRequest request);

        TestConnectionResponse TestConnection(TestConnectionRequest request);

        DiscoverShapesResponse Shapes(DiscoverSubscriberShapesRequest request);

        ReceiveDataPointResponse Receive(ReceiveDataPointRequest request);

        void Dispose(DisposeRequest request);

    }
}
