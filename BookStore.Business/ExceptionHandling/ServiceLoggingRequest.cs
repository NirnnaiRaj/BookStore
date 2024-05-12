using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.ExceptionHandling
{
    public class ServiceLoggingRequest
    {
        public int ServiceCallId { get; set; }
        public string ServiceCallGuid { get; set; }
        public string ServiceMethod { get; set; }
        public string RequestContent { get; set; }
        public string ResponseContent { get; set; }
        public DateTime RequestStartTime { get; set; }
        public DateTime RequestEndTime { get; set; }
    }
}
