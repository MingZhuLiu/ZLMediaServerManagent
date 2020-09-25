using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZLMediaServerManagent.Models.ViewDto
{

    /// <summary>
    /// 页面操作权限控制
    /// </summary>
    public class PowerDto
    {
        /// <summary>
        /// 添加按钮
        /// </summary>
        public bool Add { get; set; }

        /// <summary>
        /// 编辑按钮
        /// </summary>
        public bool Edit { get; set; }


        public bool Stop { get; set; }

        /// <summary>
        /// 删除按钮
        /// </summary>
        public bool Delete { get; set; }

        /// <summary>
        /// 授予权限菜单
        /// </summary>
        public bool RolePower { get; set; }

        /// <summary>
        /// 审核操作
        /// </summary>
        public bool Audit { get; set; }


        public bool AddDomain{get;set;}
        public bool EditDomain{get;set;}
        public bool DeleteDomain{get;set;}
        public bool AddApplication{get;set;}
        public bool EditApplication{get;set;}
        public bool DeleteApplication{get;set;}
    }
}
