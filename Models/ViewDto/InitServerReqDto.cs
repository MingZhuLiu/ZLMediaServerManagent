using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZLMediaServerManagent.Models.ViewDto
{
    public class InitServerReqDto 
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string ZLMediaServerIp { get; set; }
        public string ZLMediaServerPort { get; set; }
        public string ZLMediaServerSecret { get; set; }
    }
}
