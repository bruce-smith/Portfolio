using Amazon.Runtime.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Portfolio
{
    public static class ip
    {
        public static string GetIpAddress(HttpRequest Request)
        {
            Microsoft.Extensions.Primitives.StringValues realip;
            bool found = false;
            found = Request.Headers.TryGetValue("CF-Connecting-IP", out realip);
            if (!found)
            {
                realip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return realip;
        }

    }
}
