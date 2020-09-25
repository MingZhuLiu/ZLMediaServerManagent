using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZLMediaServerManagent.Models.ViewDto
{
    public class LayuiTreeDto
    {
        public LayuiTreeDto()
        {
            spread = true;
        }
        public string title { get; set; }
        public long id { get; set; }
        public long parentId { get; set; }
        public List<LayuiTreeDto> children { get; set; }
        public string href { get; set; }
        public bool spread { get; set; }

        public bool @checked { get; set; }

        public bool @disabled { get; set; }
    }

}
