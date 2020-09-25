using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.DataBase
{

    [Table("TbApplication")]
    public class TbApplication : BaseTable
    {

        [Required]
        [MaxLength(100)]
        public string AppName { get; set; }

        [Required]
        [ForeignKey("Domain")]
        public long DomainId{get;set;}

        [MaxLength(200)]
        public string Description { get; set; }


        public virtual TbDomain Domain { get; set; }

    }
}