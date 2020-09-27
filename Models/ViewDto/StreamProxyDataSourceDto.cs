using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLMediaServerManagent.Models.Dto;

namespace ZLMediaServerManagent.Models.ViewDto
{
    public class StreamProxyDataSourceDto
    {
        public StreamProxyDataSourceDto()
        {
            Domains = new List<DomainDto>();
            Applications = new List<ApplicationDto>();
        }
        public List<DomainDto> Domains { get; set; }
        public List<ApplicationDto> Applications { get; set; }

        public PowerDto Power { get; set; }
        public StreamProxyDto StreamProxy { get; set; }

    }
}
