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
    public class DomainAndAppController : BaseController
    {

        public DomainAndAppController(IMapper mapper, IUserService userService, IDomainAndAppService domainAndAppService)
        {
            this.mapper = mapper;
            this.userService = userService;
            this.domainAndAppService = domainAndAppService;
        }


        [HttpGet]
        public IActionResult DomainAndApp()
        {
            PowerDto dto = new PowerDto();
            var menus = userService.FindUserMenus(GetUserDto(), false);
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("DomainAndApp") && p.Url.Contains("AddDomain")).Any())
                dto.AddDomain = true;
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("DomainAndApp") && p.Url.Contains("EditDomain")).Any())
                dto.EditDomain = true;
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("DomainAndApp") && p.Url.Contains("DeleteDomain")).Any())
                dto.DeleteDomain = true;
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("DomainAndApp") && p.Url.Contains("AddApplication")).Any())
                dto.AddApplication = true;
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("DomainAndApp") && p.Url.Contains("EditApplication")).Any())
                dto.EditApplication = true;
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("DomainAndApp") && p.Url.Contains("DeleteApplication")).Any())
                dto.DeleteApplication = true;
            return View(dto);
        }

        [HttpPost]
        public dynamic DomainAndApp(QueryModel queryModel)
        {
            dynamic result;
            if (queryModel.Flag)
            {
                //查询域名
                result = domainAndAppService.QueryDomainList(queryModel);
            }
            else
            {
                result = domainAndAppService.QueryApplicationList(queryModel);
                //查询应用
            }
            return result;
        }

        [HttpGet]
        public IActionResult AddDomain()
        {
            return View();
        }
        [HttpPost]
        public BaseModel<String> AddDomain(DomainDto dto)
        {
            return domainAndAppService.AddDoamin(dto, GetUserDto());
        }


        [HttpGet]
        public IActionResult EditDomain(long domainId)
        {
            var domain = domainAndAppService.FindDomain(domainId);
            return View(domain);
        }

        [HttpPost]
        public BaseModel<String> EditDomain(DomainDto dto)
        {
            return domainAndAppService.EditDoamin(dto, GetUserDto());
        }


        [HttpPost]
        public BaseModel<String> DeleteDomain(long[] ids)
        {
            return domainAndAppService.DeleteDoamin(ids, GetUserDto());
        }



        [HttpGet]
        public IActionResult AddApplication()
        {
            return View();
        }
        [HttpPost]
        public BaseModel<String> AddApplication(ApplicationDto dto)
        {
            return domainAndAppService.AddApplication(dto, GetUserDto());
        }


        [HttpGet]
        public IActionResult EditApplication(long applicationId)
        {
            var domain = domainAndAppService.FindApplication(applicationId);
            return View(domain);
        }

        [HttpPost]
        public BaseModel<String> EditApplication(ApplicationDto dto)
        {
            return domainAndAppService.EditApplication(dto, GetUserDto());
        }


        [HttpPost]
        public BaseModel<String> DeleteApplication(long[] ids)
        {
            return domainAndAppService.DeleteApplication(ids, GetUserDto());
        }


    }
}