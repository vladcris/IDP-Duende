using AutoMapper;
using IdentityProvider.Duende.Entities.Register;

namespace IdentityProvider.Duende.Entities;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRegistrationModel, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
    }
}
