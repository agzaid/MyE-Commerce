using Microsoft.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Injection.Localization
{
    public class RewriteRules
    {
        public static void RedirectRequests(RewriteContext context)
        {
            var request = context.HttpContext.Request;
            if (request.RouteValues["area"] is null)
            {
                var path = request.Path.Value;

                var userLangs = request.Headers["Accept-Language"].ToString();
                var firstLang = userLangs.Split(',').FirstOrDefault();
                var defultCulture = string.IsNullOrEmpty(firstLang) ? "en" : firstLang[..2];

                //Add your conditions of redirecting
                if (string.IsNullOrWhiteSpace(request.RouteValues["culture"]?.ToString()))// If the url does not contain culture
                {
                    context.HttpContext.Response.Redirect($"/{defultCulture}{path}");
                }
            }
        }
    }
}
