using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.DataBase
{


    public class BaseTable
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public DateTime CreateTs { get; set; }

        [ForeignKey("CreateByUser")]
        public long CreateBy { get; set; }

        [DefaultValue(200)]
        public int State { get; set; }

        public DateTime UpdateTs { get; set; }

        [ForeignKey("UpdateByUser")]
        public long UpdateBy { get; set; }

        public virtual TbUser CreateByUser { get; set; }
        public virtual TbUser UpdateByUser { get; set; }



    }
}