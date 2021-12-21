using System;
using System.Net;

namespace DAL.Exceptions
{
    public abstract class BaseHttpException: Exception
    {
        public override string Message { get; }
        public HttpStatusCode HttpStatusCode { get; }

        protected BaseHttpException(string message, HttpStatusCode code)
        {
            Message = message;
            HttpStatusCode = code;
        }
    }
}