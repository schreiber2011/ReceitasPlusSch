using AutoMapper;
using MyRecipeBook.Application.Services.Mappings;

namespace CommonTestUtilities.Mapper;

public class MapperBuilder
{
    public static IMapper Build()
        => new AutoMapper.MapperConfiguration(option =>
            {
                option.AddProfile(new AutoMapping());
            }).CreateMapper();
}
