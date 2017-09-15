using Naveego.Pipeline.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naveego.Pipeline.Publishers.Transport
{
    public interface IDataTransport
    {
        void Send(IList<DataPoint> dataPoints);
        void Done();
    }


    internal class DoneRequest { }
    internal class DoneResponse { }

    internal class SendDataPointsRequest
    {
        public DataPoint[] DataPoints { get; set; }
    }

    internal class SendDataPointsResponse { }

}
