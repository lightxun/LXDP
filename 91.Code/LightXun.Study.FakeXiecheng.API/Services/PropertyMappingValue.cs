using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LightXun.Study.FakeXiecheng.API.Services
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; private set; }
        public PropertyMappingValue(IEnumerable<string> destinationProperties)
        {
            this.DestinationProperties = destinationProperties;
        }
    }
}
