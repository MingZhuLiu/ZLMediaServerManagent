using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ZLMediaServerManagent.Commons;
using ZLMediaServerManagent.Filters;
using ZLMediaServerManagent.Interface;
using ZLMediaServerManagent.Models;
using ZLMediaServerManagent.Models.Dto;
using ZLMediaServerManagent.Models.ViewDto;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Controllers
{
    [TypeFilter(typeof(GlobalFiler))]
    public class ZLServerController : BaseController
    {

        public ZLServerController(IMapper mapper, IUserService userService, IDomainAndAppService domainAndAppService, IZLServerService zLServerService)
        {
            this.mapper = mapper;
            this.userService = userService;
            this.domainAndAppService = domainAndAppService;
            this.zLServerService = zLServerService;
        }



        [HttpGet]
        public IActionResult Config()
        {
            return View();
        }

        [HttpPost]
        public TableQueryModel<ConfigDto> Config(QueryModel queryModel)
        {
            TableQueryModel<ConfigDto> result = new TableQueryModel<ConfigDto>();

            result.data = GloableCache.ZLMediaServerConfig.Select(p => new ConfigDto() { ConfigKey = p.Key, ConfigValue = p.Value }).ToList();
            foreach (var item in result.data)
            {
                if (item.ConfigKey == "http.notFound")
                {
                    item.ConfigValue = "<xmp>" + item.ConfigValue + "</xmp>";
                    break;
                }
            }
            result.code = 0;
            result.msg = "ok";
            result.count = result.data.Count;
            return result;
        }


        [HttpPost]
        public BaseModel<String> EditConfig(ConfigDto dto)
        {
            BaseModel<String> baseModel = new BaseModel<String>();
            try
            {
                if (dto == null || String.IsNullOrWhiteSpace(dto.ConfigKey))
                {
                    baseModel.Failed("参数错误!");
                }
                else
                {
                    var setConfigResult = GloableCache.ZLClient.setServerConfig(dto.ConfigKey, dto.ConfigValue);
                    if (setConfigResult.code == 0 && setConfigResult.changed != 0)
                    {
                        baseModel.Success("修改配置[" + dto.ConfigKey + "]成功!");
                        GloableCache.ZLMediaServerConfig = GloableCache.ZLClient.getServerConfig().data.First();//刷新一遍配置
                        // GloableCache.ZLClient.addStreamProxy()
                    }
                    else
                    {
                        baseModel.Failed("修改配置[" + dto.ConfigKey + "]失败!");
                    }
                }
            }
            catch (Exception ex)
            {
                baseModel.Failed(ex.Message);
            }
            return baseModel;
        }



        [HttpGet]
        public IActionResult StreamProxy()
        {
            StreamProxyDataSourceDto dataSourceDto = new StreamProxyDataSourceDto();
            dataSourceDto.Power = new PowerDto();
            var menus = userService.FindUserMenus(GetUserDto(), false);
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("ZLServer") && p.Url.Contains("AddStreamProxy")).Any())
                dataSourceDto.Power.Add = true;
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("ZLServer") && p.Url.Contains("EditStreamProxy")).Any())
                dataSourceDto.Power.Edit = true;
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("ZLServer") && p.Url.Contains("DeleteStreamProxy")).Any())
                dataSourceDto.Power.Delete = true;

            //判断添加条件，必须存在域名和应用才允许添加
            if (DataBaseCache.Applications.Where(p => p.State != (int)BaseStatus.Deleted && DataBaseCache.Domains.Where(q => q.Id == p.DomainId).First().State != (int)BaseStatus.Deleted).Any())
            {
                dataSourceDto.Power.Audit = true;
            }


            dataSourceDto.Domains = DataBaseCache.Domains.Where(p => p.State != (int)BaseStatus.Deleted).Select(p => mapper.Map<DomainDto>(p)).ToList();
            dataSourceDto.Applications = DataBaseCache.Applications.Where(p => p.State != (int)BaseStatus.Deleted).Select(p => mapper.Map<ApplicationDto>(p)).ToList();
            return View(dataSourceDto);
        }

        [HttpPost]
        public TableQueryModel<StreamProxyDto> StreamProxy(QueryModel queryModel)
        {
            return zLServerService.StreamProxy(queryModel);
        }


        [HttpGet]
        public IActionResult AddStreamProxy()
        {
            return View();
        }

        [HttpPost]
        public BaseModel<String> AddStreamProxy(StreamProxyDto dto)
        {
            return zLServerService.AddStreamProxy(dto, GetUserDto());
        }


        [HttpGet]
        public IActionResult EditStreamProxy(long streamProxyId)
        {

            var dataSource = new StreamProxyDataSourceDto() { StreamProxy = zLServerService.FindStreamProxy(streamProxyId) };
            return View(dataSource);
        }

        [HttpPost]
        public BaseModel<String> EditStreamProxy(StreamProxyDto dto)
        {
            return zLServerService.EditStreamProxy(dto, GetUserDto());
        }


        [HttpPost]
        public BaseModel<String> DeleteStreamProxy(long[] ids)
        {
            return zLServerService.DeleteStreamProxy(ids, GetUserDto());
        }


        [HttpGet]
        public IActionResult Play(long id)
        {
            var streamProxy = zLServerService.FindStreamProxy(id);
            var domain = domainAndAppService.FindDomain(streamProxy.DomainId);
            var application = domainAndAppService.FindApplication(streamProxy.ApplicationId);
            PlayStreamProxyDto playStreamProxy = new PlayStreamProxyDto(streamProxy, domain, application);

            return View(playStreamProxy);
        }

    }
}