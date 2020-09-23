using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLMediaServerManagent.Models;
using ZLMediaServerManagent.Models.Dto;
using ZLMediaServerManagent.Models.ViewDto;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Interface
{
    public interface IUserService
    {
        UserDto FindUserByLoginName(String loginName);
        UserDto FindUserById(long id);
        bool ValidateToken(string token, LoginPlatform loginPlatform);
        LoginResultDto<UserDto> LoginCheck(string account, string passwd, LoginPlatform loginPlatform);
        public List<MenuDto> FindUserMenus(UserDto user, bool allMenu = true);
        public List<MenuDto> FindUserMenuTree(UserDto user, bool allMenu = true);
        // bool LoginOut(string token, LoginPlatform loginPlatform);

        TableQueryModel<UserDto> GetUserList(QueryModel queryModel);

        BaseModel<String> AddUser(UserDto dto, UserDto operaUser);
        BaseModel<String> EditUser(UserDto dto, UserDto operaUser);
        BaseModel<String> DeleteUsers(long[] ids, UserDto operaUser);

    }
}
