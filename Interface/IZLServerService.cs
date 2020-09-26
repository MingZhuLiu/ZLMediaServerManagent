using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLMediaServerManagent.Models;
using ZLMediaServerManagent.Models.Dto;

namespace ZLMediaServerManagent.Interface
{
    public interface IZLServerService
    {

        TableQueryModel<StreamProxyDto> StreamProxy(QueryModel queryModel);
        StreamProxyDto FindStreamProxy(long StreamProxyId);
        BaseModel<String> AddStreamProxy(StreamProxyDto dto, UserDto owner);
        BaseModel<String> EditStreamProxy(StreamProxyDto dto, UserDto owner);
        BaseModel<String> DeleteStreamProxy(long[] ids, UserDto owner);





    }
}