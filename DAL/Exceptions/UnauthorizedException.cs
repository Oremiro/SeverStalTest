using System.Net;

namespace DAL.Exceptions
{
    public sealed class UnauthorizedException: BaseHttpException
    { 
        private const HttpStatusCode Code = HttpStatusCode.Unauthorized;

        public UnauthorizedException(string message) : base(message, Code)
        {
         
        }
        
    }
}