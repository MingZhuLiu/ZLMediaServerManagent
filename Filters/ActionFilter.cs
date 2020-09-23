using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ZLMediaServerManagent.Commons;

namespace ZLMediaServerManagent.Filters
{
    public class ActionFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //执行完成....
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

            var claim_clientId = context.HttpContext?.User?.FindFirst(ClaimTypes.Sid)?.Value;
            if (String.IsNullOrWhiteSpace(claim_clientId))
            {
                var claimsIdentity = new ClaimsIdentity("Cookie");
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString().Replace("-", "").ToLower()));
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                context.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal).Wait();
            }
            else if (!GloableCache.OnlineClients.ContainsKey(claim_clientId))
            {
                GloableCache.OnlineClients.Add(claim_clientId, new Models.ViewDto.OnlineClientTokenInfo() { });
            }

            //如果客户端没有clientId，则颁发一个clientId
            // if (!context.HttpContext.Request.Cookies.ContainsKey("clientId"))
            // {
            //     context.HttpContext.Response.Cookies.Append("clientId", Guid.NewGuid().ToString().Replace("-", "").ToLower());
            // }

            //执行中...
            if (!GloableCache.IsInitServer)
            {
                if (!(context.HttpContext.Request.Path.HasValue && context.HttpContext.Request.Path.Value.StartsWith("/Home/Init")))
                {
                    //如果没有初始化服务器数据，直接重定向到初始化配置页面
                    RedirectResult result = new RedirectResult("~/Home/Init");
                    context.Result = result;
                    return;
                }
            }


        }
    }
}