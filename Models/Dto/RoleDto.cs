using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Models.Dto
{
    public class RoleDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateTs { get; set; }
        public RoleStatus Status { get; set; }
        public List<MenuDto> Menus { get; set; }
    }
}
