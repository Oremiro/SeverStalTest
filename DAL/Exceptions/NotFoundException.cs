using System;
using System.Net;

namespace DAL.Exceptions
{
    public sealed class NotFoundException: BaseHttpException
    {
        private const HttpStatusCode Code = HttpStatusCode.NotFound;

        public NotFoundException(string message) : base(message, Code)
        {
         
        }
    }
}