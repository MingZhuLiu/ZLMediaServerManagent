using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZLMediaServerManagent.Models.Dto
{
    public class MenuRoleDto
    {
        public long MenuId { get; set; }
        public long RoleId { get; set; }

        public virtual MenuDto Menu { get; set; }
        public virtual RoleDto Role { get; set; }
    }
}
