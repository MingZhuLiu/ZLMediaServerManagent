using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Models.Dto
{
    public class MenuDto : BaseDto
    {
        public MenuDto()
        {
            Children = new List<MenuDto>();
        }

        public long? ParentId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int Order { get; set; }

        public List<MenuDto> Children { get; set; }
    }
}
