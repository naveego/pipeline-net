using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naveego.Pipeline.Publishers
{
    public class PublisherInstance
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string IconURL { get; set; }

        public IList<ShapeDefinition> Shapes { get; set; }

        public IDictionary<string, object> Settings { get; set; }
    }
}
