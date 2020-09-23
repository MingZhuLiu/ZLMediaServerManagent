using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZLMediaServerManagent.Models.ViewDto
{
    public class LoginReqDto
    {
        public string Account{get;set;}
        public string Password{get;set;}
        public bool RememberMe{get;set;}
    }
}
