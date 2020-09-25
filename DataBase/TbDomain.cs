using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.DataBase
{

    [Table("TbDomain")]
    public class TbDomain : BaseTable
    {

        [MaxLength(30)]
        public string DomainName { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public virtual ICollection<TbApplication> Applications { get; set; }

    }
}