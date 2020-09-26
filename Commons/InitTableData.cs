using System;
using System.Collections.Generic;
using AutoMapper;
using ZLMediaServerManagent.DataBase;
using ZLMediaServerManagent.Models.Dto;

namespace ZLMediaServerManagent.Commons
{
    public static class InitTableData
    {

        /// <summary>
        /// 生成菜单数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<TbMenu> GenerateMenu(long userId)
        {

            List<TbMenu> menus = new List<TbMenu>();


            var system = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = null,
                Name = "系统管理",
                Icon = null,
                Url = null,
                Order = 1,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(system);


            var dashboard = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = system.Id,
                Name = "Dashboard",
                Icon = "fa fa-tachometer",
                Url = "/Home/Dashboard",
                Order = 1,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(dashboard);



            #region  菜单管理


            var system_menu = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = system.Id,
                Name = "菜单管理",
                Icon = "fa fa-th-list",
                Url = "/Menu/Index",
                Order = 2,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(system_menu);

            var system_menu_add = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = system_menu.Id,
                Name = "新增菜单",
                Icon = null,
                Url = "/Menu/Add",
                Order = 1,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(system_menu_add);

            var system_menu_edit = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = system_menu.Id,
                Name = "编辑菜单",
                Icon = null,
                Url = "/Menu/Edit",
                Order = 2,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(system_menu_edit);

            var system_menu_delete = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = system_menu.Id,
                Name = "删除菜单",
                Icon = null,
                Url = "/Menu/Delete",
                Order = 3,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(system_menu_delete);

            #endregion



            #region  角色管理


            var system_role = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = system.Id,
                Name = "角色管理",
                Icon = "fa fa-eye",
                Url = "/Role/Index",
                Order = 3,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(system_role);

            var system_role_add = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = system_role.Id,
                Name = "新增角色",
                Icon = null,
                Url = "/Role/Add",
                Order = 1,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(system_role_add);

            var system_role_edit = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = system_role.Id,
                Name = "编辑角色",
                Icon = null,
                Url = "/Role/Edit",
                Order = 2,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(system_role_edit);

            var system_role_delete = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = system_role.Id,
                Name = "删除角色",
                Icon = null,
                Url = "/Role/Delete",
                Order = 3,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(system_role_delete);

            var system_role_menus = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = system_role.Id,
                Name = "角色权限列表",
                Icon = null,
                Url = "/Role/GetMenuTreeByRole",
                Order = 4,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(system_role_menus);

            var system_role_power = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = system_role.Id,
                Name = "角色授权",
                Icon = null,
                Url = "/Role/RolePower",
                Order = 5,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(system_role_power);
            #endregion



            #region  用户管理


            var system_user = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = system.Id,
                Name = "用户管理",
                Icon = "fa fa-user-circle",
                Url = "/User/Index",
                Order = 4,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(system_user);

            var system_user_add = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = system_user.Id,
                Name = "新增用户",
                Icon = null,
                Url = "/User/Add",
                Order = 1,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(system_user_add);

            var system_user_edit = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = system_user.Id,
                Name = "编辑用户",
                Icon = null,
                Url = "/User/Edit",
                Order = 2,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(system_user_edit);

            var system_user_delete = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = system_user.Id,
                Name = "删除用户",
                Icon = null,
                Url = "/User/Delete",
                Order = 3,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(system_user_delete);

            #endregion


            var zlserver = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = null,
                Name = "媒体服务器",
                Icon = null,
                Url = null,
                Order = 2,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(zlserver);


            var zlserver_config = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = zlserver.Id,
                Name = "服务器配置",
                Icon = "fa fa-cogs",
                Url = "/ZLServer/Config",
                Order = 1,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(zlserver_config);

            var zlserver_editConfig = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = zlserver_config.Id,
                Name = "修改配置",
                Icon = "fa fa-cogs",
                Url = "/ZLServer/EditConfig",
                Order = 1,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(zlserver_editConfig);

            #region  域名和应用
            var domainAndApp = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = zlserver.Id,
                Name = "域名和应用",
                Icon = "fa fa-bars",
                Url = "/DomainAndApp/DomainAndApp",
                Order = 2,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(domainAndApp);

            var domainAndApp_addDomain = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = domainAndApp.Id,
                Name = "添加域名",
                Icon = null,
                Url = "/DomainAndApp/AddDomain",
                Order = 1,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(domainAndApp_addDomain);

            var domainAndApp_editDomain = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = domainAndApp.Id,
                Name = "编辑域名",
                Icon = null,
                Url = "/DomainAndApp/EditDomain",
                Order = 2,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(domainAndApp_editDomain);

            var domainAndApp_deleteDomain = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = domainAndApp.Id,
                Name = "删除域名",
                Icon = null,
                Url = "/DomainAndApp/DeleteDomain",
                Order = 3,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(domainAndApp_deleteDomain);


            var domainAndApp_addApp = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = domainAndApp.Id,
                Name = "添加应用",
                Icon = null,
                Url = "/DomainAndApp/AddApplication",
                Order = 4,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(domainAndApp_addApp);

            var domainAndApp_editApp = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = domainAndApp.Id,
                Name = "编辑应用",
                Icon = null,
                Url = "/DomainAndApp/EditApplication",
                Order = 5,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(domainAndApp_editApp);

            var domainAndApp_deleteApp = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = domainAndApp.Id,
                Name = "删除应用",
                Icon = null,
                Url = "/DomainAndApp/DeleteApplication",
                Order = 6,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(domainAndApp_deleteApp);

            #endregion

            #region  拉流代理
            var zlserver_streamProxy = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = zlserver.Id,
                Name = "拉流代理",
                Icon = "fa fa-video-camera",
                Url = "/ZLServer/StreamProxy",
                Order = 3,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(zlserver_streamProxy);


            var zlserver_addStreamProxy = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = zlserver_streamProxy.Id,
                Name = "添加拉流代理",
                Icon = null,
                Url = "/ZLServer/AddStreamProxy",
                Order = 1,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(zlserver_addStreamProxy);


            var zlserver_editStreamProxy = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = zlserver_streamProxy.Id,
                Name = "编辑拉流代理",
                Icon = null,
                Url = "/ZLServer/EditStreamProxy",
                Order = 2,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(zlserver_editStreamProxy);

            var zlserver_deleteStreamProxy = new TbMenu()
            {
                Id = Tools.NewID,
                ParentId = zlserver_streamProxy.Id,
                Name = "删除拉流代理",
                Icon = null,
                Url = "/ZLServer/DeleteStreamProxy",
                Order = 3,
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
            menus.Add(zlserver_deleteStreamProxy);



            #endregion

            return menus;
        }

        /// <summary>
        /// 生成角色数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static TbRole GenerateRole(long userId)
        {
            return new TbRole()
            {
                Id = Tools.NewID,
                Name = "超级管理员",
                Description = "拥有系统最高权限!",
                State = (int)Models.Enums.BaseStatus.Normal,
                CreateTs = DateTime.Now,
                UpdateTs = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
            };
        }
    }
}
