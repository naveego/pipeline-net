using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Naveego.Pipeline.Protocol;
using Naveego.Pipeline.Publishers;
using Naveego.Pipeline.Publishers.Transport;
using Naveego.Pipeline.Logging;

namespace PipelineTest
{
    public class Program
    {

        public class TestPublisher : AbstractPublisher
        {
            public override DiscoverShapesResponse Shapes(DiscoverPublisherShapesRequest request)
            {
                throw new NotImplementedException();
            }

            public override PublishResponse Publish(PublishRequest request, IDataTransport dataTransport)
            {
                throw new NotImplementedException();
            }
        }

        static void Main(string[] args)
        {

            var pub = new TestPublisher();
            pub.Logger = LoggerFactory.Combine(
                LoggerFactory.NewConsoleLogger());

            pub.Logger.LogInfo("Hi");
            Console.ReadLine();

        }
    }
}
