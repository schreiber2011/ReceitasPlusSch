using AutoMapper;
using MyRecipeBook.Communication.Responses;

namespace MyRecipeBook.Application.Services.Mappings;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<Communication.Requests.RequestRegisterUserJson, Domain.Entities.User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }

    private void DomainToResponse()
    {
        CreateMap<Domain.Entities.User, ResponseUserProfileJson>();
    }
}
