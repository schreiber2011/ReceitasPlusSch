using AutoMapper;
using CommonTestUtilities.IdEncryption;
using MyRecipeBook.Application.Services.Mappings;

namespace CommonTestUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
    {
        var idEncripter = IdEncripterBuilder.Build();

        return new AutoMapper.MapperConfiguration(option =>
        {
            option.AddProfile(new AutoMapping(idEncripter));
        }).CreateMapper();
    }

}
