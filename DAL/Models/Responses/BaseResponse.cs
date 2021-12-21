using System.Net;

namespace DAL.Models.Responses
{
    public class BaseResponse
    {
        public virtual HttpStatusCode StatusCode { get; set; }
        public virtual string Message { get; set; }
    }
}