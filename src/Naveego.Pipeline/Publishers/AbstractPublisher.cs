﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naveego.Pipeline.Logging;
using Naveego.Pipeline.Protocol;
using Naveego.Pipeline.Publishers.Transport;

namespace Naveego.Pipeline.Publishers
{

    public abstract class AbstractPublisher : IPublisher
    {
        private ILogger _logger = NullLogger.Instance;

        public ILogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public virtual void Dispose(DisposeRequest request) { }
        
        public virtual InitializeResponse Init(InitializePublisherRequest request)
        {
            return new InitializeResponse
            {
                Success = true,
                Message = "Not Implemented"
            };
        }

        public abstract DiscoverShapesResponse Shapes(DiscoverPublisherShapesRequest request);

        public abstract PublishResponse Publish(PublishRequest request, IDataTransport dataTransport);
        
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
