using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.DataBase
{

    [Table("TbUserRole")]
    public class TbUserRole
    {
        [Required]
        [ForeignKey("User")]
        public long UserId { get; set; }

        [Required]
        [ForeignKey("Role")]
        public long RoleId { get; set; }
        public virtual TbUser User { get; set; }
        public virtual TbRole Role { get; set; }

    }
}