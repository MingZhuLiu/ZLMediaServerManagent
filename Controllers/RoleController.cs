using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZLMediaServerManagent.Filters;
using ZLMediaServerManagent.Interface;
using ZLMediaServerManagent.Models;
using ZLMediaServerManagent.Models.Dto;
using ZLMediaServerManagent.Models.ViewDto;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Controllers
{
    public class RoleController : BaseController
    {
        public RoleController(IRoleService roleService, IMenuService menuService, IUserService userService)
        {
            this.roleService = roleService;
            this.menuService = menuService;
            this.userService = userService;
        }

        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult Index()
        {
            PowerDto dto = new PowerDto();
            var menus = userService.FindUserMenus(GetUserDto(), false);
            if (menus.Where(p => !String.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("Role") && p.Url.Contains("Add")).Any())
                dto.Add = true;
            if (menus.Where(p => !String.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("Role") && p.Url.Contains("Edit")).Any())
                dto.Edit = true;
            if (menus.Where(p => !String.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("Role") && p.Url.Contains("Delete")).Any())
                dto.Delete = true;
            if (menus.Where(p => !String.IsNullOrWhiteSpace(p.Url) && p.Url.Contains("Role") && p.Url.Contains("RolePower")).Any())
                dto.RolePower = true;
            return View(dto);
        }


        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public TableQueryModel<RoleDto> Index(QueryModel query)
        {
            query.limit = 9999999;
            return roleService.QueryRoleList(query);
        }

        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public List<LayuiTreeDto> GetMenuTreeByRole(long roleId)
        {
            List<LayuiTreeDto> trees = new List<LayuiTreeDto>();


            var list = menuService.GetMenuList(new QueryModel() { limit = 99999999, page = 1 }).data as List<MenuDto>;
            list = list.OrderBy(p => p.Order).ToList();
            //list存放所有的菜单.
            //只允许用户授权自己才有权限的菜单


            list = list.Where(p => p.State == (int)MenuStatus.Normal).ToList();


            var nowSelected = menuService.FindMenusByRole(new RoleDto() { Id = roleId });
            List<MenuDto> nowSelectResult = new List<MenuDto>();
            foreach (var item in nowSelected)
            {
                nowSelectResult.Add(new MenuDto()
                {
                    CreateTs = item.CreateTs,
                    Icon = item.Icon,
                    Id = item.Id,
                    Name = item.Name,
                    Order = item.Order,
                    ParentId = item.ParentId,
                    Url = item.Url
                });
                ExpandTreeViewNode(item.Children, ref nowSelectResult);
            }
            foreach (var item in list)
            {
                if (item.ParentId.HasValue)
                    continue;
                LayuiTreeDto dto = new LayuiTreeDto();
                dto.@checked = nowSelectResult.Where(p => p.Id == item.Id).Any() ? true : false;
                dto.children = findAllParents(item, list, nowSelectResult);
                dto.id = item.Id;
                dto.title = item.Name;
                trees.Add(dto);
            }

            foreach (var item in trees)
            {
                item.@checked = false;
                DebugTree(item.children);
            }

            return trees;

        }



        [HttpGet]
        [TypeFilter(typeof(GlobalFiler))]
        public IActionResult Add()
        {
            return View();
        }


        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public BaseModel<String> Add(RoleDto dto)
        {
            BaseModel<String> baseModel = new BaseModel<String>();
            try
            {
                if (roleService.AddRole(dto, GetUserDto()))
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
            RoleDto dto = roleService.FindRole(id);
            return View(dto);
        }


        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public BaseModel<String> Edit(RoleDto dto)
        {
            BaseModel<String> baseModel = new BaseModel<String>();
            try
            {

                if (roleService.EditRole(dto, GetUserDto()))
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
                if (roleService.DeleteRole(ids, GetUserDto()))
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




        [HttpPost]
        [TypeFilter(typeof(GlobalFiler))]
        public BaseModel<String> RolePower(RolePowerDto dto)
        {
            BaseModel<String> baseModel = new BaseModel<String>();
            try
            {
                var role = roleService.FindRole(dto.RoleId);


                List<LayuiTreeDto> result = new List<LayuiTreeDto>();
                if (dto.Menus != null)
                    foreach (var item in dto.Menus)
                    {
                        result.Add(new LayuiTreeDto()
                        {
                            @checked = item.@checked,
                            disabled = item.disabled,
                            href = item.href,
                            id = item.id,
                            spread = item.spread,
                            title = item.title
                        });
                        ExpandTreeViewNode(item.children, ref result);
                    }


                if (!roleService.FindUserRoles(GetUserDto()).Where(p => p.Name.Contains("超级管理员")).Any())
                {
                    var myMenus = userService.FindUserMenus(GetUserDto(), false);
                    result = result.Where(p => myMenus.Select(q => q.Id).ToList().Contains(p.id)).ToList();
                }





                if (roleService.RolePower(dto.RoleId, result.Select(p => p.id).ToArray()))
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


        /// <summary>
        /// 展开用户所有权限
        /// </summary>
        /// <param name="node"></param>
        /// <param name="result"></param>
        private void DebugTree(List<LayuiTreeDto> nodes)
        {
            if (nodes != null)
                foreach (LayuiTreeDto item in nodes)
                {
                    if (item.children != null && item.children.Count != 0)
                        item.@checked = false;
                    DebugTree(item.children);
                }
        }

        /// <summary>
        /// 展开用户所有权限
        /// </summary>
        /// <param name="node"></param>
        /// <param name="result"></param>
        private void ExpandTreeViewNode(List<MenuDto> node, ref List<MenuDto> result)
        {
            if (node == null)
                return;
            foreach (MenuDto item in node)
            {
                result.Add(new MenuDto()
                {
                    CreateTs = item.CreateTs,
                    Icon = item.Icon,
                    Id = item.Id,
                    Name = item.Name,
                    Order = item.Order,
                    ParentId = item.ParentId,
                    Url = item.Url
                });
                ExpandTreeViewNode(item.Children, ref result);
            }
        }

        /// <summary>
        /// 找到所有的父节点
        /// </summary>
        /// <returns></returns>
        private List<LayuiTreeDto> findAllParents(MenuDto menu, List<MenuDto> allData, List<MenuDto> nowSelected)
        {
            List<LayuiTreeDto> treeList = new List<LayuiTreeDto>();
            if (allData != null)
            {
                foreach (var item in allData)
                {
                    if (item.ParentId == menu.Id)
                    {
                        LayuiTreeDto myTree = new LayuiTreeDto();
                        myTree.@checked = nowSelected.Where(p => p.Id == item.Id).Any();
                        myTree.children = findAllParents(item, allData, nowSelected);
                        myTree.id = item.Id;
                        myTree.title = item.Name;
                        treeList.Add(myTree);
                    }

                }
            }
            if (treeList.Count == 0)
                treeList = null;
            return treeList;
        }


        private void ExpandTreeViewNode(List<LayuiTreeDto> node, ref List<LayuiTreeDto> result)
        {
            if (node == null)
                return;
            foreach (LayuiTreeDto item in node)
            {
                result.Add(new LayuiTreeDto()
                {
                    @checked = item.@checked,
                    disabled = item.disabled,
                    href = item.href,
                    id = item.id,
                    spread = item.spread,
                    title = item.title
                });
                ExpandTreeViewNode(item.children, ref result);
            }
        }
    }
}