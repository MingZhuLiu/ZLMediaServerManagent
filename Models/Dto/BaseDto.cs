using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ZLMediaServerManagent.Models.Enums;

namespace ZLMediaServerManagent.Models.Dto
{
    public class BaseDto
    {
       
        public long Id { get; set; }

        public DateTime CreateTs { get; set; }

        public long CreateBy { get; set; }

        public int State { get; set; }

        public DateTime UpdateTs { get; set; }

        public long UpdateBy { get; set; }

        public virtual UserDto CreateByUser { get; set; }
        public virtual UserDto UpdateByUser { get; set; }

    }


}
