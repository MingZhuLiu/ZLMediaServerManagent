using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using ZLMediaServerManagent.Commons;
using ZLMediaServerManagent.DataBase;
using ZLMediaServerManagent.Filters;
using ZLMediaServerManagent.Hubs;
using ZLMediaServerManagent.Interface;
using ZLMediaServerManagent.Models;
using ZLMediaServerManagent.Models.Dto;
using ZLMediaServerManagent.Models.ViewDto;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IMapper mapper, IHubContext<MessageHub> messageHub, ZLDataBaseContext zLDataBase, IUserService userService)
        {
            _logger = logger;
            this.messageHub = messageHub;
            this.zLDataBase = zLDataBase;
            this.mapper = mapper;
            this.userService = userService;
        }
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult Index()
        {
            var model = new LayoutViewDto() { Menus = userService.FindUserMenuTree(GetUserDto()) };
            return View(model);
        }

        [SkipGlobalActionFilter]
        public IActionResult Privacy()
        {
            return View();
        }

        [SkipGlobalActionFilter]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [SkipGlobalActionFilter]
        [HttpGet]
        public IActionResult Init()
        {
            if (GloableCache.IsInitServer)
            {
                return RedirectToAction("Login");
            }
            else
                return View();
        }


        [SkipGlobalActionFilter]
        [HttpPost]
        public async Task<BaseModel<string>> InitAsync(InitServerReqDto req)
        {
            BaseModel<String> result = new BaseModel<string>();
            try
            {
                await BrowserReceiveLoaderMessageAsync("开始校验数据...", Enums.MessageType.Info);
                if (String.IsNullOrWhiteSpace(req.Account) || String.IsNullOrWhiteSpace(req.Password))
                {
                    await BrowserReceiveLoaderMessageAsync("登录信息填写错误,请重新填写!", Enums.MessageType.Info);
                    return result.Failed("登录信息填写错误,请重新填写!");
                }

                await BrowserReceiveLoaderMessageAsync("开始验证流媒体服务器<a href=\"http://" + req.ZLMediaServerIp + ":" + req.ZLMediaServerPort + "\" target=\"_blank\">http://" + req.ZLMediaServerIp + ":" + req.ZLMediaServerPort + "</a>", Enums.MessageType.Warning);
                var client = new STRealVideo.Lib.ZLClient("http://" + req.ZLMediaServerIp + ":" + req.ZLMediaServerPort, req.ZLMediaServerSecret);
                var configs = client.getServerConfig();
                if (configs != null && configs.code == 0 && configs.data != null && configs.data.Count != 0 && configs.data[0].ContainsKey("api.secret"))
                {
                    //连接服务器成功,开始生成数据
                    await BrowserReceiveLoaderMessageAsync("服务器验证通过!", Enums.MessageType.Success);
                    await BrowserReceiveLoaderMessageAsync("开始创建数据库基础信息...", MessageType.Warning);
                }
                else
                {
                    await BrowserReceiveLoaderMessageAsync("连接流媒体服务器失败:" + configs.msg, Enums.MessageType.Failed);
                    return result.Failed("连接流媒体服务器失败:" + configs.msg);
                }
            }
            catch (Exception ex)
            {
                await BrowserReceiveLoaderMessageAsync("验证流媒体服务器异常，异常信息:" + ex.Message, Enums.MessageType.Failed);
                return result.Failed("验证流媒体服务器异常，异常信息:" + ex.Message);
            }

            try
            {
                var userId = Tools.NewID;
                var date = DateTime.Now;
                var user = new TbUser()
                {
                    Id = userId,
                    LoginName = req.Account,
                    LoginPasswd = req.Password,
                    Name = "超级管理员",
                    Sex = Enums.SexEnum.Man,
                    Tel = null,
                    Address = null,
                    CreateBy = userId,
                    UpdateBy = userId,
                    CreateTs = date,
                    UpdateTs = date,
                    State = (int)BaseStatus.Normal
                };
                zLDataBase.Users.Add(user);
                var menus = InitTableData.GenerateMenu(userId);
                zLDataBase.Menus.AddRange(menus);
                var role = InitTableData.GenerateRole(userId);
                zLDataBase.Roles.Add(role);
                menus.ForEach(v =>
                {
                    zLDataBase.MenuRoles.Add(new TbMenuRole() { MenuId = v.Id, RoleId = role.Id });
                });
                zLDataBase.UserRoles.Add(new TbUserRole() { RoleId = role.Id, UserId = userId });

                var config_serverIp = new TbConfig()
                {
                    ConfigKey = ConfigKeys.ZLMediaServerIp,
                    ConfigValue = req.ZLMediaServerIp
                };

                var config_serverPort = new TbConfig()
                {
                    ConfigKey = ConfigKeys.ZLMediaServerPort,
                    ConfigValue = req.ZLMediaServerPort
                };

                var config_serverSec = new TbConfig()
                {
                    ConfigKey = ConfigKeys.ZLMediaServerSecret,
                    ConfigValue = req.ZLMediaServerSecret
                };
                zLDataBase.Configs.Add(config_serverIp);
                zLDataBase.Configs.Add(config_serverPort);
                zLDataBase.Configs.Add(config_serverSec);

                await BrowserReceiveLoaderMessageAsync("正在提交数据库事物...", Enums.MessageType.Warning);
                await zLDataBase.SaveChangesAsync();
                await BrowserReceiveLoaderMessageAsync("恭喜,系统已经初始化完毕,Enjoy It!", Enums.MessageType.Success);
                Startup.ReadDataBase2Cache(zLDataBase);

                LoginLogic(mapper.Map<UserDto>(user));//执行登录逻辑


                Thread.Sleep(3000);
                return result.Success("初始化完毕!");
            }
            catch (Exception ex)
            {
                await BrowserReceiveLoaderMessageAsync("初始化数据库发生异常:" + ex.Message, Enums.MessageType.Failed);
                return result.Failed("初始化数据库发生异常:" + ex.Message);
            }
        }

        [SkipGlobalActionFilter]
        [HttpGet]
        public IActionResult Login()
        {
            if (!String.IsNullOrWhiteSpace(GetUserDto()?.LoginName))
            {
                //登录状态-重定向到首页
                return RedirectToAction("Index");
            }
            else
            {
                var lastaccount = GetCookies("lastAccount");
                ViewData["lastAccount"] = lastaccount;
                return View();
            }
        }

        [SkipGlobalActionFilter]
        [HttpPost]
        public async Task<BaseModel<string>> LoginAsync(LoginReqDto reqDto)
        {
            BaseModel<String> result = new BaseModel<string>();
            var loginValidate = userService.LoginCheck(reqDto.Account, reqDto.Password, LoginPlatform.Web);
            if (loginValidate.Flag)
            {
                //登录验证成功..
                //如果登录成功要先看之前有没有同账号登录，如果有的话，让他下线
                var lastLogin = GloableCache.OnlineClients.Values.Where(p => p.ClientId != null && p.User != null && p.User.Id == loginValidate.Data.Id).FirstOrDefault();
                if (lastLogin != null)
                {

                    //发消息让这个人下线.
                    await messageHub.Clients.Client(lastLogin.SignarlRId).SendAsync("CleanCookieAndExit");
                    lastLogin.User = null;
                }

                //设置缓存，cache什么的
                LoginLogic(loginValidate.Data);
                if (reqDto.RememberMe)
                {
                    SetCookies("lastAccount", reqDto.Account, 60 * 24 * 365);
                }
                result.Success("登录成功!");
            }
            else
            {
                result.Failed(loginValidate.Msg);
            }
            return result;
        }

        [SkipGlobalActionFilter]
        public IActionResult Loginout()
        {
            LoginOutLogic();
            return RedirectToAction("Login");
        }

        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult Dashboard()
        {
            return View();
        }

    }
}