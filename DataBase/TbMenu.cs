using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.DataBase
{

    [Table("TbMenu")]
    public class TbMenu : BaseTable
    {

        [ForeignKey("Parent")]
        public long? ParentId { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }


        [MaxLength(30)]
        public string Icon { get; set; }

        [MaxLength(200)]
        public string Url { get; set; }

        public int Order { get; set; }

        public virtual TbMenu Parent { get; set; }

        public virtual ICollection<TbMenu> Childrens { get; set; }

        public virtual ICollection<TbMenuRole> MenuRoles { get; set; }

    }
}