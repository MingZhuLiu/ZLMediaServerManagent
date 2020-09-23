using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZLMediaServerManagent.Commons;
using ZLMediaServerManagent.Interface;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Filters
{
    public class GlobalFiler : Attribute, IAuthorizationFilter
    {
        private IUserService userService;
        public GlobalFiler(IUserService userService)
        {
            this.userService = userService;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {



            var isSkip = false;
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                isSkip = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                  .Any(a => a.GetType().Equals(typeof(SkipGlobalActionFilterAttribute)));
            }
            if (isSkip) return;

            var clientId = context.HttpContext?.User?.FindFirst(ClaimTypes.Sid)?.Value;
            // context.HttpContext.Request.Cookies.TryGetValue(CookieKeys.WebToken + "", out string value);
            if (!GloableCache.OnlineClients.ContainsKey(clientId) || GloableCache.OnlineClients[clientId] == null || GloableCache.OnlineClients[clientId].User == null)
            {
                RedirectResult result = new RedirectResult("~/Home/Login");
                context.Result = result;
                return;
            }
            //再判断是否有菜单操作权限
            var reqPath = context.HttpContext.Request.Path.ToString();
            if (reqPath != null && reqPath.EndsWith("/"))
                reqPath = reqPath.Substring(0, reqPath.Length - 1);
            if (reqPath == "/Home/Index" || reqPath == "/Home/Dashboard")
                return;
            var user = GloableCache.OnlineClients[clientId].User;
            if (user == null)
            {
                //未登录  无权
                context.Result = new RedirectResult("/Home/AuthError");
                return;
            }
            var menus = userService.FindUserMenus(user, false);
            if (!menus.Where(p => p.Url != null && p.Url.ToLower().Contains(reqPath.ToLower())).Any())
            {
                //已登录 无权
                context.Result = new RedirectResult("/Home/AuthError");
                return;
            }
            else
            {
                //继续
                return;

            }
        }
    }
}
