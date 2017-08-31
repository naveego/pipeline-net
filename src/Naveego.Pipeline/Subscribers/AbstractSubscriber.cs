using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naveego.Pipeline.Protocol;

namespace Naveego.Pipeline.Subscribers
{
    public abstract class AbstractSubscriber : ISubscriber
    {
        public virtual void Dispose(DisposeRequest request) { }
        

        public virtual InitializeResponse Init(InitializeSubscriberRequest request)
        {
            return new InitializeResponse
            {
                Success = true,
                Message = "Not Implemented"
            };
        }

        public abstract ReceiveDataPointResponse Receive(ReceiveDataPointRequest request);

        public abstract DiscoverShapesResponse Shapes(DiscoverSubscriberShapesRequest request);
     

        public virtual TestConnectionResponse TestConnection(TestConnectionRequest request)
        {
            return new TestConnectionResponse
            {
                Success = true,
                Message = "Not Implemented"
            };
        }
    }
}
