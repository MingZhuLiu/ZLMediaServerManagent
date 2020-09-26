using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Models.Dto
{
    public class StreamProxyDto : BaseDto
    {

        public long DomainId { get; set; }

        public long ApplicationId { get; set; }

        public string StreamId { get; set; }

        public string StreamName { get; set; }

        public string PullStreamUrl { get; set; }

        public bool EnableHLS { get; set; }

        public bool EnableMP4 { get; set; }

        public RTPType RtpType { get; set; }

        public string Description { get; set; }

        public virtual DomainDto Domain { get; set; }
        public virtual ApplicationDto Application { get; set; }


        public virtual int WatchCount { get; set; }
        public virtual int WatchTotalCount { get; set; }
        public virtual string ShowStatus { get; set; }
        public virtual string ShowStreamMediaJson{ get; set; }
        public virtual string DomainName{ get; set; }
        public virtual string ApplicationName{ get; set; }

        public virtual bool isNeedRePullStreamProxy{get;set;}

    }



}
