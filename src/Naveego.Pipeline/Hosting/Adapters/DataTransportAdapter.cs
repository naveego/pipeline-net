using Naveego.Pipeline.Publishers.Transport;
using System.Collections.Generic;
using System.Linq;

namespace Naveego.Pipeline.Hosting
{
    /// <summary>
    /// Adapter to fulfil the IDataTransport contract using an IClient instance.
    /// </summary>
    internal class DataTransportAdapter : IDataTransport
    {
        private readonly IClient _client;

        public DataTransportAdapter(IClient client)
        {
            _client = client;
        }

        public void Done()
        {
            var resp = _client.Invoke<DoneResponse>("Done", new DoneRequest());
        }

        public void Send(IList<DataPoint> dataPoints)
        {
            var resp = _client.Invoke<SendDataPointsResponse>("SendDataPoints", new SendDataPointsRequest
            {
                DataPoints = dataPoints.ToArray()
            });
        }
    }
}
