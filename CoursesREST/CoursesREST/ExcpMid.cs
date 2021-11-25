using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace CoursesREST
{
    public class ExcpMid
    {
        private readonly RequestDelegate _next;
        public ExcpMid(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex) { }
        }
    }
}
