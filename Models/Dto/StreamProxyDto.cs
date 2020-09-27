using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLMediaServerManagent.Commons;
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


        /// <summary>
        /// 观看数量
        /// </summary>
        public virtual int WatchCount { get; set; }

        /// <summary>
        /// 总观看数量
        /// </summary>
        public virtual int WatchTotalCount { get; set; }

        /// <summary>
        /// 前端显示的状态
        /// </summary>
        public virtual string ShowStatus { get; set; }

        /// <summary>
        /// 媒体信息的json
        /// </summary>
        public virtual string ShowStreamMediaJson { get; set; }

        /// <summary>
        /// 前端显示的域名
        /// </summary>
        public virtual string DomainName { get; set; }

        /// <summary>
        /// 前端显示的应用
        /// </summary>
        public virtual string ApplicationName { get; set; }

        /// <summary>
        /// 是否需要重新拉流
        /// </summary>
        public virtual bool isNeedRePullStreamProxy { get; set; }

        

    }



}
