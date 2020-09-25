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

namespace ZLMediaServerManagent.Controllers
{
    [TypeFilter(typeof(GlobalFiler))]
    public class ZLServerController : BaseController
    {

        public ZLServerController(IMapper mapper, IUserService userService)
        {
            this.mapper = mapper;
            this.userService = userService;
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


        //EditConfig

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




    }
}