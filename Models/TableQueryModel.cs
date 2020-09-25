using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZLMediaServerManagent.Models
{
    public class TableQueryModel<T>
    {
        public int code { get; set; }
        public string msg { get; set; }
        public int count { get; set; }
        public List<T> data { get; set; }

        // public int total => count;


        public TableQueryModel<T> Success(List<T> data)
        {
            if (data == null || data.Count == 0)
            {
                return Failed("暂无数据!");
            }
            this.code = 0;
            this.msg = "查询成功!";
            this.data = data;
            return this;
        }

        public TableQueryModel<T> Failed(String msg)
        {
            this.code = 1;
            this.msg = msg;
            this.data = null;
            return this;
        }
        public TableQueryModel<T> Failed(Exception ex)
        {
            return Failed(ex.Message);
        }
    }
}
