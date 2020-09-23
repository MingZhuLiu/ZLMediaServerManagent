using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLMediaServerManagent.Models;
using ZLMediaServerManagent.Models.Dto;

namespace ZLMediaServerManagent.Interface
{
    public interface IRoleService
    {
        List<RoleDto> FindUserRoles(UserDto user);

        TableQueryModel<RoleDto> QueryRoleList(QueryModel query);


        bool AddRole(RoleDto dto, UserDto user);

        RoleDto FindRole(long id);

        bool EditRole(RoleDto dto, UserDto user);

        bool DeleteRole(long[] ids, UserDto user);

        bool RolePower(long roleId, long[] menusId);
        bool SetUserRole(long userId, long[] roleIds);
        List<RoleDto> GetCanPowerRoles(UserDto user);
    }
}