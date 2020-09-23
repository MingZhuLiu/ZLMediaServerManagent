using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLMediaServerManagent.Models.Dto;

namespace ZLMediaServerManagent.Models.ViewDto
{
    public class OnlineClientTokenInfo
    {
        /// <summary>
        /// 客户端Id,GUID，保存在浏览器Cookie中
        /// </summary>
        /// <value></value>
        public string ClientId { get; set; }

        /// <summary>
        /// Token信息，带有用户数据
        /// </summary>
        /// <value></value>
        public string WebToken { get; set; }


        /// <summary>
        /// SignalR客户端连接Id
        /// </summary>
        /// <value></value>
        public string SignarlRId { get; set; }
        

        public UserDto User{get;set;}
    }
}
