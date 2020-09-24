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
