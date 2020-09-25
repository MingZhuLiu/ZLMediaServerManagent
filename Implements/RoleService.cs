using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLMediaServerManagent.Commons;
using ZLMediaServerManagent.DataBase;
using ZLMediaServerManagent.Interface;
using ZLMediaServerManagent.Models;
using ZLMediaServerManagent.Models.Dto;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Implements
{
    public class RoleService : IRoleService
    {
        private readonly ZLDataBaseContext dbContext;
        private IMapper mapper;
        public RoleService(ZLDataBaseContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }



        public List<RoleDto> FindUserRoles(UserDto user)
        {
            var roles = DataBaseCache.Roles.Where(p => DataBaseCache.UserRoles.Where(p => p.UserId == user.Id).Select(p => p.RoleId).ToList().Contains(p.Id)).Select(p => mapper.Map<RoleDto>(p)).ToList();
            return roles;
        }

        public TableQueryModel<RoleDto> QueryRoleList(QueryModel queryModel)
        {
            TableQueryModel<RoleDto> tableQueryModel = new TableQueryModel<RoleDto>();
            try
            {
                var query = DataBaseCache.Roles.Where(p => p.State != (int)RoleStatus.Deleted).AsQueryable();


                if (!String.IsNullOrWhiteSpace(queryModel.keyword))
                {
                    query = query.Where(p => p.Name.Contains(queryModel.keyword)).AsQueryable();
                }
                tableQueryModel.count = query.Count();
                if (!String.IsNullOrWhiteSpace(queryModel.field) && !String.IsNullOrWhiteSpace(queryModel.order))
                {
                    query = query.SortBy(queryModel.field + " " + queryModel.order.ToUpper());
                }
                query = query.Skip((queryModel.page - 1) * queryModel.limit);
                query = query.Take(queryModel.limit);

                tableQueryModel.code = 0;
                tableQueryModel.data =  query.Select(p=>mapper.Map<RoleDto>(p)).ToList();

            }
            catch (Exception ex)
            {
                tableQueryModel.code = 1;
                tableQueryModel.msg = ex.Message;
            }
            return tableQueryModel;
        }

        public bool AddRole(RoleDto dto, UserDto user)
        {
            if (dto == null || String.IsNullOrWhiteSpace(dto.Name))
                return false;
            if (dto.Name.Contains("管理员"))
                return false;
            TbRole role = new TbRole();
            role.Id = Tools.NewID;
            role.CreateBy = user.Id;
            role.CreateTs = DateTime.Now;
            role.Description = dto.Description;
            role.Name = dto.Name;
            role.State = (int)dto.State;
            role.UpdateBy = user.Id;
            role.UpdateTs = role.CreateTs;

            dbContext.Roles.Add(role);
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                DataBaseCache.Roles.Add(role);
            }
            return flag;
        }

        public RoleDto FindRole(long id)
        {
            var dbModel = DataBaseCache.Roles.Where(p => p.Id == id).FirstOrDefault();
            if (dbModel != null)
                return mapper.Map<RoleDto>(dbModel);
            else
                return null;
        }

        public bool EditRole(RoleDto dto, UserDto user)
        {
            if (dto == null || String.IsNullOrWhiteSpace(dto.Name))
                return false;
            var dbModel = dbContext.Roles.Where(p => p.Id == dto.Id).FirstOrDefault();
            if (dbModel == null)
                return false;



            dbModel.Name = dto.Name;
            dbModel.Description = dto.Description;
            dbModel.State = (int)dto.State;
            dbModel.UpdateBy = user.Id;
            dbModel.UpdateTs = DateTime.Now;

            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                //更新缓存
                DataBaseCache.Roles.Remove(DataBaseCache.Roles.Where(p => p.Id == dbModel.Id).First());
                DataBaseCache.Roles.Add(dbModel);
            }
            return flag;
        }


        public bool DeleteRole(long[] ids, UserDto user)
        {
            if (ids == null)
                return false;
            var roles = dbContext.Roles.Where(p => ids.Contains(p.Id)).ToList();
            foreach (var item in roles)
            {

                item.State = (int)RoleStatus.Deleted;
                item.UpdateBy = user.Id;
                item.UpdateTs = DateTime.Now;
            }
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                //删除缓存
                DataBaseCache.Roles = dbContext.Roles.ToList();
            }
            return flag;
        }

        public bool RolePower(long roleId, long[] menusIds)
        {
            var list = dbContext.MenuRoles.Where(p => p.RoleId == roleId).ToList();
            if (list != null && list.Count != 0)
                dbContext.MenuRoles.RemoveRange(list);
            dbContext.SaveChanges();
            if (menusIds != null)
            {
                foreach (var item in menusIds)
                {
                    TbMenuRole mr = new TbMenuRole();
                    mr.MenuId = item;
                    mr.RoleId = roleId;
                    dbContext.MenuRoles.Add(mr);
                }
            }
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                DataBaseCache.MenuRoles = dbContext.MenuRoles.ToList();
            }
            return flag;
        }

        public bool SetUserRole(long userId, long[] roleIds)
        {
            var removes = dbContext.UserRoles.Where(p => p.UserId == userId).ToList();
            foreach (var item in removes)
            {
                dbContext.Remove(item);
            }
            dbContext.SaveChanges();
            foreach (var item in roleIds)
            {
                TbUserRole role = new TbUserRole();
                role.UserId = userId;
                role.RoleId = item;
                dbContext.UserRoles.Add(role);
            }
            dbContext.SaveChanges();
            DataBaseCache.UserRoles = dbContext.UserRoles.ToList();
            return true;
        }

        public List<RoleDto> GetCanPowerRoles(UserDto user)
        {
            var hasRoles = FindUserRoles(user);
            if (hasRoles.Where(p => p.Name == "超级管理员").Any())
            {
                //顶级超级管理员
                return QueryRoleList(new QueryModel() { page = 1, limit = 999999999 }).data;
            }
            else
            {
                //普通用户,将自己的权限返回回去
                return FindUserRoles(user);
            }
        }
    }
}