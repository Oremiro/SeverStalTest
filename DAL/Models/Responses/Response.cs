namespace DAL.Models.Responses
{
    public class Response<T>: BaseResponse
    {
        public Response()
        {
        }
        public Response(T data)
        {
            this.Succeeded = true;
            this.Message = string.Empty;
            this.Errors = null;
            this.Data = data;
        }
        public T Data { get; set; }
        public bool? Succeeded { get; set; }
        public string[] Errors { get; set; }
        public override string Message { get; set; }
    }
}