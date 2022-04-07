using AutoMapper;
using SessionService.DatabaseObject;
using SessionService.Entities;

namespace SessionService.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddCustomerDto, Customer>();
            CreateMap<UpdateCustomerDto, Customer>();
        }
    }
}
