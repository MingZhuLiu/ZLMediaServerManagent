using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.DataBase
{

    [Table("TbRole")]
    public class TbRole : BaseTable
    {

        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public virtual ICollection<TbMenuRole> MenusRoles { get; set; }
        public virtual ICollection<TbUserRole> UserRoles { get; set; }
    }
}