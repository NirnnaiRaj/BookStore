using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Business.ExceptionHandling
{
    public class ErrorLoggingRequest
    {
        public int ErrorLogId { get; set; }
        public string ServiceCallGuid { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorStacktrace { get; set; }
        public DateTime RequestStartTime { get; set; }
        public DateTime RequestEndTime { get; set; }
    }
}
