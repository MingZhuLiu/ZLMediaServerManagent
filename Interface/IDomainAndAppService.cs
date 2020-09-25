using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZLMediaServerManagent.Models;
using ZLMediaServerManagent.Models.Dto;

namespace ZLMediaServerManagent.Interface
{
    public interface IDomainAndAppService
    {

        TableQueryModel<DomainDto> QueryDomainList(QueryModel query);
        TableQueryModel<ApplicationDto> QueryApplicationList(QueryModel query);


        DomainDto FindDomain(long domainId);
        BaseModel<String> AddDoamin(DomainDto dto, UserDto owner);
        BaseModel<String> EditDoamin(DomainDto dto, UserDto owner);
        BaseModel<String> DeleteDoamin(long[] ids, UserDto owner);



        ApplicationDto FindApplication(long applicationId);
        BaseModel<String> AddApplication(ApplicationDto dto, UserDto owner);
        BaseModel<String> EditApplication(ApplicationDto dto, UserDto owner);
        BaseModel<String> DeleteApplication(long[] ids, UserDto owner);


    }
}