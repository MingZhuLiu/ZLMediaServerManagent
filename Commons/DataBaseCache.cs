using System.Collections.Generic;
using ZLMediaServerManagent.DataBase;

namespace ZLMediaServerManagent.Commons
{
    public static class DataBaseCache
    {
        public static List<TbMenu> Menus = new List<TbMenu>();
        public static List<TbConfig> Configs = new List<TbConfig>();
        public static List<TbMenuRole> MenuRoles = new List<TbMenuRole>();
        public static List<TbRole> Roles = new List<TbRole>();
        public static List<TbUser> Users = new List<TbUser>();
        public static List<TbUserRole> UserRoles = new List<TbUserRole>();
    }
}