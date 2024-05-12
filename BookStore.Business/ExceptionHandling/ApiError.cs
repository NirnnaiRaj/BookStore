namespace BookStore.Business
{
    public class ApiError
    {
        public ApiError() { }

        public ApiError(string Message)
        {
            this.Message = Message;
        }
        public ApiError(string Message, AppErrorCode code) {

            this.ErrorCode = code;
            this.Message = Message;
        }
        public AppErrorCode ErrorCode { get;  set; }

        public string Message { get;  set; }
    }
}
