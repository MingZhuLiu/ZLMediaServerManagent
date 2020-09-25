using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace  ZLMediaServerManagent.Models
{
    public class QueryModel
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int limit { get; set; }

        /// <summary>
        /// 查询关键字
        /// </summary>
        public string keyword { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string field { get; set; }

        /// <summary>
        /// 正序/倒序  asc/desc
        /// </summary>
        public string order { get; set; }


        /// <summary>
        /// 父级id
        /// </summary>
        public long? parentId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Flag { get; set; }

    }
}
