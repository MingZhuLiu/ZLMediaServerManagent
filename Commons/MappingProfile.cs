using AutoMapper;
using ZLMediaServerManagent.DataBase;
using ZLMediaServerManagent.Models.Dto;

namespace ZLMediaServerManagent.Commons
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TbUser, UserDto>().ReverseMap();
            CreateMap<TbMenu, MenuDto>().ReverseMap();
            CreateMap<TbRole, RoleDto>().ReverseMap();
            CreateMap<TbUserRole, UserRoleDto>().ReverseMap();
            CreateMap<TbMenuRole, MenuRoleDto>().ReverseMap();
            CreateMap<TbConfig, ConfigDto>().ReverseMap();

        }
    }
}
