using System.Collections.Generic;
using ZLMediaServerManagent.Models.Dto;

namespace ZLMediaServerManagent.Models.ViewDto
{
     public class MenuEditDatasourceDto
    {
        public MenuDto Menu { get; set; }
        public List<MenuDto> Menus { get; set; }
    }
}