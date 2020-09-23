using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLMediaServerManagent.Models;
using ZLMediaServerManagent.Models.Dto;

namespace ZLMediaServerManagent.Interface
{
    public interface IMenuService
    {
        public List<MenuDto> FindMenusByRole(RoleDto role);

        public TableQueryModel<MenuDto> GetMenuList(QueryModel query);

        public MenuDto FindMenu(long id);
        public bool AddMenu(MenuDto dto,UserDto owner);
        public bool EditMenu(MenuDto dto,UserDto owner);

        public bool DeleteMenu(long[] ids,UserDto owner);

    }
}