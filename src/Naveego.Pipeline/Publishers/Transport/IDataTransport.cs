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
    }
}
