using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Models.Dto
{
    public class DomainDto : BaseDto
    {



        public string DomainName { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ApplicationDto> Applications { get; set; }

    }



}
