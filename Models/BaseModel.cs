using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace  ZLMediaServerManagent.Models
{
    public class BaseModel<T>
    {
        public bool Flag { get; set; }
        public String Msg { get; set; }
        public T Data { get; set; }

        public BaseModel()
        {

        }
        public BaseModel(bool flag, string msg)
        {
            this.Flag = flag;
            this.Msg = msg;
        }
        public BaseModel<T> Success(string msg)
        {
            this.Flag = true;
            this.Msg = msg;
            return this;
        }

        public BaseModel<T> Failed(Exception ex)
        {
            this.Flag = false;
            this.Msg = ex.Message;
            return this;
        }
        public BaseModel<T> Failed(string msg)
        {
            this.Flag = false;
            this.Msg = msg;
            return this;
        }
    }

}
