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
    public class MenuService : IMenuService
    {
        private readonly ZLDataBaseContext dbContext;
        private IMapper mapper;
        public MenuService(ZLDataBaseContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }


        public List<MenuDto> FindMenusByRole(RoleDto role)
        {
            var menuIds = DataBaseCache.MenuRoles.Where(p => p.RoleId == role.Id).Select(p => p.MenuId).ToList();

            return DataBaseCache.Menus.Where(p => menuIds.Contains(p.Id)).Select(p => mapper.Map<MenuDto>(p)).ToList();
        }

        public TableQueryModel<MenuDto> GetMenuList(QueryModel queryModel)
        {
            TableQueryModel<MenuDto> tableQueryModel = new TableQueryModel<MenuDto>();
            try
            {
                List<MenuDto> menus = new List<MenuDto>();
                var query = DataBaseCache.Menus.Where(p => p.State != (int)MenuStatus.Deleted).AsQueryable();
                // if (queryModel.Flag == false)
                // {
                //     // queryModel.Flag==false 前台根据需要查询对应表格.
                //     // queryModel.Flag==true 其他业务逻辑要查询所有表格
                //     if (queryModel.parentId != 0)
                //     {
                //         query = query.Where(p => p.ParentId == queryModel.parentId);
                //     }
                //     else
                //     {
                //         query = query.Where(p => p.ParentId == null);
                //     }
                // }

                if (!String.IsNullOrWhiteSpace(queryModel.keyword))
                {
                    query = query.Where(p => p.Name.Contains(queryModel.keyword)).AsQueryable();
                }

                if (!String.IsNullOrWhiteSpace(queryModel.field) && !String.IsNullOrWhiteSpace(queryModel.order))
                {
                    query = query.SortBy(queryModel.field + " " + queryModel.order.ToUpper());
                }
                else
                {
                    query = query.SortBy("CreateTs ASC");
                }
                tableQueryModel.count = query.Count();
                query = query.Skip((queryModel.page - 1) * queryModel.limit);
                query = query.Take(queryModel.limit);
                var list = query.ToList();
                list.ForEach(p => menus.Add(mapper.Map<MenuDto>(p)));

                tableQueryModel.code = 0;
                // tableQueryModel.data = menus.ToTree<MenuDto>(
                //     (r, c) => { return c.ParentId == 0 || !c.ParentId.HasValue; },
                //     (r, c) =>
                //     {
                //         return r.Id == c.ParentId;
                //     },
                //     (r, datalist) =>
                //     {
                //         r.Children.AddRange(datalist);
                //     }
                //     );

                tableQueryModel.data = menus;
            }
            catch (Exception ex)
            {
                tableQueryModel.code = 1;
                tableQueryModel.msg = ex.Message;
            }
            return tableQueryModel;
        }


        public MenuDto FindMenu(long id)
        {
            var dbModel = DataBaseCache.Menus.Where(p => p.Id == id).FirstOrDefault();
            if (dbModel != null)
                return mapper.Map<MenuDto>(dbModel);
            else
                return null;
        }


        public bool AddMenu(MenuDto dto, UserDto owner)
        {
            if (dto == null || String.IsNullOrWhiteSpace(dto.Name))
                return false;
            dto.Id = Tools.NewID;
            dto.CreateTs = DateTime.Now;
            dto.UpdateTs = dto.CreateTs;
            dto.CreateBy = owner.Id;
            dto.UpdateBy = owner.Id;

            var dbModel = mapper.Map<TbMenu>(dto);
            dbContext.Menus.Add(dbModel);
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                DataBaseCache.Menus.Add(dbModel);
            }
            return flag;
        }

        public bool EditMenu(MenuDto dto, UserDto owner)
        {
            if (dto == null || String.IsNullOrWhiteSpace(dto.Name))
                return false;
            var dbModel = dbContext.Menus.Where(p => p.Id == dto.Id).FirstOrDefault();
            if (dbModel == null)
                return false;
            dbModel.Icon = dto.Icon;
            dbModel.Name = dto.Name;
            dbModel.Order = dto.Order;
            dbModel.State = (int)dto.State;
            dbModel.ParentId = dto.ParentId;
            dbModel.Url = dto.Url;
            dbModel.UpdateBy = owner.Id;
            dbModel.UpdateTs = DateTime.Now;
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                DataBaseCache.Menus.Remove(DataBaseCache.Menus.Where(p => p.Id == dto.Id).First());
                DataBaseCache.Menus.Add(dbModel);
            }
            return flag;
        }

        public bool DeleteMenu(long[] ids, UserDto owner)
        {
            if (ids == null)
                return false;
            var menus = dbContext.Menus.Where(p => ids.Contains(p.Id)).ToList();
            foreach (var item in menus)
            {
                item.State = (int)MenuStatus.Deleted;
                item.UpdateBy = owner.Id;
                item.UpdateTs = DateTime.Now;
            }
            var flag = dbContext.SaveChanges() > 0 ? true : false;
            if (flag)
            {
                DataBaseCache.Menus = dbContext.Menus.ToList();
            }
            return flag;
        }
    }
}