using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.DataBase
{

    [Table("TbUser")]
    public class TbUser : BaseTable
    {

        [MaxLength(20)]
        [Required]
        public string LoginName { get; set; }

        [MaxLength(30)]
        [Required]
        public string LoginPasswd { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public SexEnum Sex { get; set; }

        [MaxLength(50)]
        public string Tel { get; set; }


        [MaxLength(20)]
        public string Address { get; set; }

    }
}