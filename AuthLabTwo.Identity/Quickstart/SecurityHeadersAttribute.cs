﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IdentityServerHost.Quickstart.UI
{
    public class SecurityHeadersAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result;
            if (result is ViewResult)
            {
                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
                {
                    context.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                }

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
                if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
                {
                    context.HttpContext.Response.Headers.Add("X-Frame-Options", "ALLOW-FROM https://localhost:44450");
                }

                //// https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
                //var csp = "default-src 'self'; object-src 'none'; frame-ancestors https://localhost:44450; sandbox allow-forms allow-same-origin allow-scripts 'self' 'unsafe-inline'; base-uri 'self' 'nonce-646489489';";

                //if (!context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy"))
                //{
                //    context.HttpContext.Response.Headers.Add("Content-Security-Policy", csp);
                //}

                //if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy"))
                //{
                //    context.HttpContext.Response.Headers.Add("X-Content-Security-Policy", csp);
                //}

                // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
                var referrer_policy = "no-referrer";
                if (!context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy"))
                {
                    context.HttpContext.Response.Headers.Add("Referrer-Policy", referrer_policy);
                }
            }
        }
    }
}
