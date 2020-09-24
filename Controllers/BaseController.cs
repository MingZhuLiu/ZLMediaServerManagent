using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ZLMediaServerManagent.Commons;
using ZLMediaServerManagent.DataBase;
using ZLMediaServerManagent.Hubs;
using ZLMediaServerManagent.Interface;
using ZLMediaServerManagent.Models.Dto;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Controllers
{
    public class BaseController : Controller
    {
        protected IHubContext<MessageHub> messageHub;
        protected ZLDataBaseContext zLDataBase;
        protected IUserService userService;
        protected IMenuService menuService;
        protected IRoleService roleService;

        protected IMapper mapper;
        public BaseController()
        {

        }

        public UserDto GetUserDto()
        {
            var clientId = HttpContext?.User?.FindFirst(ClaimTypes.Sid)?.Value;
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                if (GloableCache.OnlineClients.ContainsKey(clientId))
                {
                    return GloableCache.OnlineClients[clientId]?.User;
                }
            }
            return null;
        }

        public void LoginLogic(UserDto user)
        {
            // var userIdClaim = new Claim(ClaimTypes.PrimarySid, user.Id.ToString());
            // var userLoginNameClaim = new Claim(ClaimTypes.NameIdentifier, user.LoginName);
            // var userNameClaim = new Claim(ClaimTypes.Name, user.Name);
            // HttpContext.User.Claims.Append(userIdClaim);
            // HttpContext.User.Claims.Append(userLoginNameClaim);
            // HttpContext.User.Claims.Append(userNameClaim);
            var clientId = HttpContext?.User?.FindFirst(ClaimTypes.Sid)?.Value;
            if (GloableCache.OnlineClients.ContainsKey(clientId))
                GloableCache.OnlineClients[clientId].User = user;
            else
            {
                GloableCache.OnlineClients.Add(clientId, new Models.ViewDto.OnlineClientTokenInfo() { User = user });
            }
        }

        public void LoginOutLogic()
        {
            var clientId = HttpContext?.User?.FindFirst(ClaimTypes.Sid)?.Value;
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                if (GloableCache.OnlineClients.ContainsKey(clientId))
                {
                    GloableCache.OnlineClients[clientId].User = null;
                    GloableCache.OnlineClients[clientId].WebToken = null;
                }

            }
        }

        public async Task BrowserReceiveLoaderMessageAsync(string msg, MessageType messageType)
        {
            var clientId = HttpContext?.User?.FindFirst(ClaimTypes.Sid)?.Value;
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                if (GloableCache.OnlineClients.ContainsKey(clientId) && !string.IsNullOrWhiteSpace(GloableCache.OnlineClients[clientId].SignarlRId))
                {
                    await messageHub.Clients.Client(GloableCache.OnlineClients[clientId].SignarlRId).SendAsync("BrowserReceiveLoaderMessage", msg, messageType);
                }
            }
        }


        /// <summary>
        /// 设置本地cookie
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>  
        /// <param name="minutes">过期时长，单位：分钟</param>      
        protected void SetCookies(string key, string value, int minutes = 30)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(minutes)
            });
        }

        protected void SetCookies(CookieKeys key, string value, int minutes = 30)
        {
            SetCookies(key + "", value, minutes);
        }


        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键</param>
        protected void DeleteCookies(string key)
        {
            HttpContext.Response.Cookies.Delete(key);
        }

        protected void DeleteCookies(CookieKeys key)
        {
            DeleteCookies(key + "");
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        protected string GetCookies(string key)
        {
            HttpContext.Request.Cookies.TryGetValue(key, out string value);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }

        protected string GetCookies(CookieKeys key)
        {
            return GetCookies(key + "");
        }
    }
}