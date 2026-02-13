using Mapster;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.Services.Mappings;

public static class MapConfigurations
{
    public static void Configure()
    {
        TypeAdapterConfig<RequestRegisterUserJson, Domain.Entities.User>
            .NewConfig()
            .Ignore(dest => dest.Password);
    }
}
