using System.Net;

namespace DAL.Exceptions
{
    public sealed class InternalServerErrorException : BaseHttpException
    {
        private const HttpStatusCode Code = HttpStatusCode.InternalServerError;

        public InternalServerErrorException(string message) : base(message, Code)
        {
        }
    }
}