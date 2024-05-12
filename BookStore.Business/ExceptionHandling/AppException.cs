using System;
using System.Globalization;

namespace BookStore.Business
{
    public class AppException: Exception
    {
        public AppErrorCode ErrorCode { get; set; }


        public AppException() : base() { }

        public AppException(string message, AppErrorCode code) : base(message) {
            this.ErrorCode = code;
        }

        public AppException(string message, AppErrorCode code, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
            this.ErrorCode = code;
        }
    }
}
