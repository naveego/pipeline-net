using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naveego.Pipeline.Protocol;
using Naveego.Pipeline.Publishers;
using Naveego.Pipeline.Publishers.Transport;
using NUnit.Framework;

namespace Naveego.Pipeline.Logging
{
    public class MockPublisher : AbstractPublisher
    {
        public override DiscoverShapesResponse Shapes(DiscoverPublisherShapesRequest request)
        {
            return new DiscoverShapesResponse();
        }

        public override PublishResponse Publish(PublishRequest request, IDataTransport dataTransport)
        {
            return new PublishResponse();
        }
    }

    [TestFixture]
    public class LoggerFactoryTests
    {

        [Test]
        public void Can_Combine_Loggers()
        {

           new MockPublisher().Logger = LoggerFactory.Combine(
                LoggerFactory.NewConsoleLogger(),
                LoggerFactory.NewFileLogger());
        }

    }
}
