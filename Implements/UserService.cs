using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLMediaServerManagent.Commons;
using ZLMediaServerManagent.DataBase;
using ZLMediaServerManagent.Interface;
using ZLMediaServerManagent.Models;
using ZLMediaServerManagent.Models.Dto;
using ZLMediaServerManagent.Models.ViewDto;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Implements
{
    public class UserService : IUserService
    {

        private readonly ZLDataBaseContext dbContext;
        private IMapper mapper;
        private IRoleService roleService;
        private IMenuService menuService;
        public UserService(ZLDataBaseContext dbContext, IMapper mapper, IRoleService roleService, IMenuService menuService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.roleService = roleService;
            this.menuService = menuService;
        }

        public UserDto FindUserByLoginName(string loginName)
        {
            // var dto = RedisHelper.Instance.GetHash<UserDto>(loginName);
            // if (dto == null)
            // {
            //     var dbModel = dbContext.TbUser.Include(p => p.TbUserRole).Where(p => p.LoginName == loginName).FirstOrDefault();
            //     if (dbModel != null)
            //     {
            //         dto = mapper.Map<UserDto>(dbModel);
            //         //dto.Roles = dbContext.TbRole.Where(p => dbModel.TbUserRole.Select(p => p.RoleId).ToList().Contains(p.Id)).Select(p => mapper.Map<RoleDto>(p)).ToList();// dbModel.TbUserRole.Select(p => p.Role).Select(p => mapper.Map<RoleDto>(p)).ToList();
            //         RedisHelper.Instance.SetHash<UserDto>(loginName, dto);
            //     }
            // }


            return DataBaseCache.Users.Where(p => p.LoginName == loginName).Select(p => mapper.Map<UserDto>(p)).FirstOrDefault();
        }

        public LoginResultDto<UserDto> LoginCheck(string account, string passwd, LoginPlatform loginPlatform)
        {
            LoginResultDto<UserDto> result = new LoginResultDto<UserDto>();

            try
            {
                if (String.IsNullOrWhiteSpace(account) || String.IsNullOrWhiteSpace(passwd))
                {
                    result.Failed("用户名/密码不能为空!");
                    return result;
                }


                //passwd = RSAHelper.Instance.Encrypt(account+passwd);
                var user = FindUserByLoginName(account);
                if (user == null || user.State == UserStatus.Deleted || user.LoginPasswd != passwd)
                {
                    result.Failed("用户名/密码错误!");
                    return result;
                }
                if (user.State == UserStatus.Forbid)
                {
                    result.Failed("您的账号已被停用,请联系单位管理员!");
                    return result;
                }

                result.Success("登录成功!");
                result.Data=user;
                //走到这里说明验证通过

            }
            catch (Exception ex)
            {
                result.Failed(ex);
            }


            return result;
        }

        public bool ValidateToken(string token, LoginPlatform loginPlatform)
        {
            return Tools.ValidateToken(token, loginPlatform);
        }



        public List<MenuDto> FindUserMenus(UserDto user, bool allMenu = true)
        {
            List<MenuDto> menus = new List<MenuDto>();
            var roles = roleService.FindUserRoles(user);

            foreach (var item in roles)
            {
                var roleMenus = menuService.FindMenusByRole(item);
                foreach (var menu in roleMenus)
                {
                    if (!menus.Where(p => p.Id == menu.Id).Any())
                        if (allMenu == true || (allMenu == false && menu.State == (int)MenuStatus.Normal))
                            menus.Add(menu);
                }
            }
            return menus;
        }

        public List<MenuDto> FindUserMenuTree(UserDto user, bool allMenu = true)
        {
            var menus = FindUserMenus(user, allMenu);

            menus = menus.ToTree<MenuDto>(
                    (r, c) => { return c.ParentId == 0 || !c.ParentId.HasValue; },
                    (r, c) =>
                    {
                        return r.Id == c.ParentId;
                    },
                    (r, datalist) =>
                    {
                        r.Children.AddRange(datalist);
                    }
                    );
            return menus;

        }



        public TableQueryModel<UserDto> GetUserList(QueryModel queryModel)
        {
            TableQueryModel<UserDto> tableQueryModel = new TableQueryModel<UserDto>();
            try
            {
                List<UserDto> menus = new List<UserDto>();
                var query = DataBaseCache.Users.Where(p => p.State != (int)BaseStatus.Deleted).AsQueryable();
                if (!String.IsNullOrWhiteSpace(queryModel.keyword))
                {
                    query = query.Where(p => p.Name.Contains(queryModel.keyword)
                    || p.Address.Contains(queryModel.keyword)
                    || p.Tel.Contains(queryModel.keyword)
                    || p.LoginName.Contains(queryModel.keyword)
                    ).AsQueryable();
                }


                tableQueryModel.count = query.Count();
                query = query.Skip((queryModel.page - 1) * queryModel.limit);
                if (!String.IsNullOrWhiteSpace(queryModel.field) && !String.IsNullOrWhiteSpace(queryModel.order))
                {
                    query = query.SortBy(queryModel.field + " " + queryModel.order.ToUpper());
                }
                query = query.Take(queryModel.limit);

                var list = query.ToList();
                list.ForEach(p => { var dto = mapper.Map<UserDto>(p); menus.Add(mapper.Map<UserDto>(dto)); });

                tableQueryModel.code = 0;
                tableQueryModel.data = menus;
            }
            catch (Exception ex)
            {
                tableQueryModel.code = 1;
                tableQueryModel.msg = ex.Message;
            }
            return tableQueryModel;
        }

        public BaseModel<String> AddUser(UserDto dto, UserDto operaUser)
        {
            BaseModel<String> result = new BaseModel<string>();
            if (String.IsNullOrWhiteSpace(dto.Name))
                return result.Failed("姓名不能为空!");

            if (String.IsNullOrWhiteSpace(dto.LoginName))
                return result.Failed("账号不能为空!");
            if (String.IsNullOrWhiteSpace(dto.LoginPasswd))
            {
                return result.Failed("密码不能为空!");
            }
            else
            {
                dto.LoginPasswd = RSAHelper.Instance.Encrypt(dto.LoginPasswd);
            }


            if (DataBaseCache.Users.Where(p => p.LoginName == dto.LoginName).Any())
                return result.Failed("用户名已存在!");

            if (dto.TempRoleId == 0)
            {
                //用户的角色设置的是空,不允许
                return result.Failed("角色错误!");
            }
            else
            {
                //检查是否超越操作者的权限
                var operaRole = roleService.GetCanPowerRoles(operaUser).Select(p => p.Id).ToArray();
                if (!operaRole.Contains(dto.TempRoleId.Value))
                {
                    return result.Failed("角色错误,禁止越权操作!");
                }
            }


            TbUser sysUser = new TbUser();
            sysUser = mapper.Map<TbUser>(dto);
            sysUser.Id = Tools.NewID;
            sysUser.CreateTs = DateTime.Now;
            sysUser.CreateBy = operaUser.Id;
            sysUser.UpdateBy = operaUser.Id;
            sysUser.UpdateTs = sysUser.CreateTs;

            dbContext.Users.Add(sysUser);
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                DataBaseCache.Users.Add(sysUser);
                roleService.SetUserRole(sysUser.Id, new long[] { dto.TempRoleId.Value });
                result.Success("保存成功!");
            }
            else
            {
                result.Success("保存失败!");
            }

            return result;
        }


        public BaseModel<String> EditUser(UserDto dto, UserDto operaUser)
        {
            BaseModel<String> result = new BaseModel<string>();
            if (String.IsNullOrWhiteSpace(dto.Name))
                return result.Failed("姓名不能为空!");
            if (String.IsNullOrWhiteSpace(dto.LoginName))
                return result.Failed("账号不能为空!");
            var dbModel = dbContext.Users.Where(p => p.Id == dto.Id).FirstOrDefault();
            if (String.IsNullOrWhiteSpace(dto.LoginPasswd))
            {

            }
            else
            {
                dbModel.LoginPasswd = dto.LoginPasswd;
            }

            if (DataBaseCache.Users.Where(p => p.LoginName == dto.LoginName && p.Id != dto.Id).Any())
                return result.Failed("用户名已存在!");
            if (dto.TempRoleId == 0)
            {
                //用户的角色设置的是空,不允许
                return result.Failed("角色错误!");
            }
            else
            {
                //检查是否超越操作者的权限
                var operaRole = roleService.GetCanPowerRoles(operaUser).Select(p => p.Id).ToArray();
                if (!operaRole.Contains(dto.TempRoleId.Value))
                {
                    return result.Failed("角色错误,禁止越权操作!");
                }
            }

            dbModel.LoginName = dto.LoginName;
            dbModel.Address = dto.Address;
            dbModel.Name = dto.Name;
            dbModel.Sex = dto.Sex;
            dbModel.State = (int)dto.State;
            dbModel.Tel = dto.Tel;
            dbModel.UpdateBy=operaUser.Id;
            dbModel.UpdateTs=DateTime.Now;

            var flag = dbContext.SaveChanges() > 0 ? true : false;
            var flag1 = roleService.SetUserRole(dbModel.Id, new long[] { dto.TempRoleId.Value });
            if (flag || flag1)
            {
                DataBaseCache.Users.Remove(DataBaseCache.Users.Where(p => p.Id == dbModel.Id).First());
                DataBaseCache.Users.Add(dbModel);
                result.Success("保存成功!");
            }
            else
            {
                result.Success("保存失败!");
            }

            return result;
        }

        public UserDto FindUserById(long id)
        {
            var dbModel = DataBaseCache.Users.Where(p => p.Id == id).FirstOrDefault();
            if (dbModel != null)
                return mapper.Map<UserDto>(dbModel);
            else
                return null;
        }

        public BaseModel<string> DeleteUsers(long[] ids, UserDto operaUser)
        {
            BaseModel<string> result = new BaseModel<string>();
            if (ids == null || ids.Length == 0)
                return result.Failed("参数错误!");

            var users = dbContext.Users.Where(p => ids.Contains(p.Id)).ToList();
            foreach (var item in users)
            {
                item.State = (int)UserStatus.Deleted;
                item.UpdateBy=operaUser.Id;
                item.UpdateTs=DateTime.Now;
            }
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                DataBaseCache.Users=dbContext.Users.ToList();
                result.Success("删除成功!");
            }
            else
            {
                result.Failed("删除失败!");
            }
            return result;

        }



    }
}