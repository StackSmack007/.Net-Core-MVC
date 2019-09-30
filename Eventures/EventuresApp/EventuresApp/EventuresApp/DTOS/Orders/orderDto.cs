using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventuresApp.DTOS.Orders
{
    public class OrderDto
    {
        public string EventName { get; set; }

        public DateTime OrderedOn { get; set; }

        public string CustomerUserName { get; set; }
    }
}
