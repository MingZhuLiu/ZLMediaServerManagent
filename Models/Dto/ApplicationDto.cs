using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Models.Dto
{
    public class ApplicationDto : BaseDto
    {

        public string AppName { get; set; }

        public long DomainId { get; set; }

        public string Description { get; set; }

        public virtual DomainDto Domain { get; set; }

    }



}
