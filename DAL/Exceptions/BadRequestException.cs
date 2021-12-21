using System.Net;

namespace DAL.Exceptions
{
    public sealed class BadRequestException : BaseHttpException
    {
        private const HttpStatusCode Code = HttpStatusCode.BadRequest;

        public BadRequestException(string message) : base(message, Code)
        {
        }
    }
}