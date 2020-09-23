using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.DataBase
{

    [Table("TbConfig")]
    public class TbConfig
    {
        [Required]
        [Key]
        [MaxLength(100)]
        public string ConfigKey { get; set; }

        public string ConfigValue { get; set; }

       
    }
}