using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.DataBase
{

    [Table("TbStreamProxy")]
    public class TbStreamProxy : BaseTable
    {
        [Required]
        [ForeignKey("Domain")]
        public long DomainId { get; set; }

        [Required]
        [ForeignKey("Application")]
        public long ApplicationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string StreamId { get; set; }

        [MaxLength(30)]
        public string StreamName { get; set; }

        [MaxLength(200)]
        public string PullStreamUrl { get; set; }

        // [DefaultValue(false)]
        // public bool EnableRTSP { get; set; }

        // [DefaultValue(false)]
        // public bool EnableRTMP { get; set; }

        [DefaultValue(false)]
        public bool EnableHLS { get; set; }

        [DefaultValue(false)]
        public bool EnableMP4 { get; set; }

        [DefaultValue(0)]
        public int RtpType { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }


        public virtual TbDomain Domain { get; set; }
        public virtual TbApplication Application { get; set; }

    }
}