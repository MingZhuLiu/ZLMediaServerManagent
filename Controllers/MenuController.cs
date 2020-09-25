using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
    public class MenuController : BaseController
    {
        public MenuController(IMenuService menuService, IUserService userService)
        {
            this.menuService = menuService;
            this.userService = userService;
        }

        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult Index()
        {

            PowerDto dto = new PowerDto();
            var menus = userService.FindUserMenus(GetUserDto(), false);
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("Menu") && p.Url.Contains("Add")).Any())
                dto.Add = true;
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("Menu") && p.Url.Contains("Edit")).Any())
                dto.Edit = true;
            if (menus.Where(p => !string.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("Menu") && p.Url.Contains("Delete")).Any())
                dto.Delete = true;
            return View(dto);
        }

        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public TableQueryModel<MenuDto> Index(QueryModel query)
        {
            return menuService.GetMenuList(query);
        }

        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult Add()
        {
            MenuEditDatasourceDto dto = new MenuEditDatasourceDto();
            var data = menuService.GetMenuList(new QueryModel() { limit = 99999999, page = 1,  });
            dto.Menus = (List<MenuDto>)data.data;
            return View(dto);
        }


        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public BaseModel<String> Add(MenuDto dto)
        {
            BaseModel<String> baseModel = new BaseModel<String>();
            try
            {
                if (menuService.AddMenu(dto, GetUserDto()))
                {
                    baseModel.Success("保存成功!");
                }
                else
                {
                    baseModel.Failed("保存失败!");
                }
            }
            catch (Exception ex)
            {
                baseModel.Failed(ex.Message);
            }
            return baseModel;
        }


        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult Edit(long id)
        {
            MenuEditDatasourceDto dto = new MenuEditDatasourceDto();
            dto.Menu = menuService.FindMenu(id);
            var data = menuService.GetMenuList(new QueryModel() { limit = 99999999, page = 1,});
            dto.Menus = (List<MenuDto>)data.data;
            return View(dto);
        }


        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public BaseModel<String> Edit(MenuDto dto)
        {
            BaseModel<String> baseModel = new BaseModel<String>();
            try
            {
                if (menuService.EditMenu(dto, GetUserDto()))
                {
                    baseModel.Success("保存成功!");
                }
                else
                {
                    baseModel.Failed("保存失败!");
                }
            }
            catch (Exception ex)
            {
                baseModel.Failed(ex.Message);
            }
            return baseModel;
        }


        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public BaseModel<String> Delete(long[] ids)
        {
            BaseModel<String> baseModel = new BaseModel<String>();
            try
            {
                if (menuService.DeleteMenu(ids, GetUserDto()))
                {
                    baseModel.Success("删除成功!");
                }
                else
                {
                    baseModel.Failed("删除失败!");
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