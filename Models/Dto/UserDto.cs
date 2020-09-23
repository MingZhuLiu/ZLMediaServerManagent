using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Models.Dto
{
    public class UserDto
    {
        public long Id { get; set; }
        public string LoginName { get; set; }
        public string LoginPasswd { get; set; }
        public string Name { get; set; }
        public SexEnum Sex { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public UserStatus State { get; set; }
        public DateTime CreateTs { get; set; }

        public string NickName { get; set; }
        public string Position { get; set; }
        public List<UserRoleDto> TbUserRole { get; set; }


        public long? TempRoleId { get; set; }
    }
}
