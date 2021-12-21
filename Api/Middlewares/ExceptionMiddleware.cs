using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using DAL.Exceptions;
using DAL.Models.Responses;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BaseHttpException httpException)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int) httpException.HttpStatusCode;

                var result = new BaseResponse
                {
                    Message = httpException.Message ?? "",
                    StatusCode = httpException.HttpStatusCode
                };
                
               
                await response.WriteAsJsonAsync(result);
            }
            catch (Exception exception)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var result = new BaseResponse
                {
                    Message = exception.Message ?? "",
                    StatusCode = HttpStatusCode.InternalServerError
                };
                
                await response.WriteAsJsonAsync(result);
            }
        }
    }
}