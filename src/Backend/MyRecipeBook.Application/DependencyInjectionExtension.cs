using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.Criptography;
using MyRecipeBook.Application.Services.Mappings;
using MyRecipeBook.Application.UseCases.Login.DoLogin;
using MyRecipeBook.Application.UseCases.User.Profile;
using MyRecipeBook.Application.UseCases.User.Register;

namespace MyRecipeBook.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        AddPasswordEncrypter(services, configuration);
        AddAutoMapper(services);
        AddUseCases(services);
    }

    private static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddScoped(option => new AutoMapper.MapperConfiguration(option =>
        {
            option.AddProfile(new AutoMapping());
        }).CreateMapper());
    }

    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
    }

    private static void AddPasswordEncrypter(this IServiceCollection services, IConfiguration configuration)
    {
        var salt = configuration.GetValue<string>("Settings:Password:salt");
        services.AddScoped(options => new PasswordEncripter(salt!));
    }
}
