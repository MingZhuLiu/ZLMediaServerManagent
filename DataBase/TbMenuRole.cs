using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.DataBase
{

    [Table("TbMenuRole")]
    public class TbMenuRole
    {
        [Required]
        [ForeignKey("Menu")]
        public long MenuId { get; set; }

        [Required]
        [ForeignKey("Role")]

        public long RoleId { get; set; }


        public virtual TbMenu Menu { get; set; }
        public virtual TbRole Role { get; set; }

    }
}