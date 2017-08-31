using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naveego.Pipeline.Protocol;
using Naveego.Pipeline.Publishers.Transport;

namespace Naveego.Pipeline.Publishers
{
    public interface IPublisher
    {

        InitializeResponse Init(InitializePublisherRequest request);

        TestConnectionResponse TestConnection(TestConnectionRequest request);

        DiscoverShapesResponse Shapes(DiscoverPublisherShapesRequest request);

        PublishResponse Publish(PublishRequest request, IDataTransport dataTransport);

        void Dispose(DisposeRequest request);
        
    }
}
