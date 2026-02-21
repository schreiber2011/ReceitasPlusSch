using MyRecipeBook.API.Converters;
using MyRecipeBook.API.Filters;
using MyRecipeBook.API.Middleware;
using MyRecipeBook.Application;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//AddJsonOptions
//    options JsonSerializerOptions Converters Add new StringConverter
// This AddJsonOptions above in comments is necessary so the space removal for name works,
// but it is breaking the register custom error responses
//WebApi.Test.User.Register.RegisterUserTest.PostUser_WhenNameIsEmpty_ShouldBeBadRequestWithNameEmptyError(culture: "en")
//  Source: RegisterUserTest.cs line 35
//  Duration: 549 ms
//  Message: 
//System.InvalidOperationException : The requested operation requires an element of type 'Array', but the target element has type 'Object'.
// The problem is that the custom exception aren't being used, instead the controller do automatic
// checks and return the default error response, which is different from the custom one, and the test is expecting the custom one, so it is breaking the test. 
// The issues happen only with RegisterUse but not with the New DoLogin controller
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options =>
{
    options.Filters.Add(typeof(ExceptionFilter));
});

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrateDatabase();

await app.RunAsync();

void MigrateDatabase()
{
    if (app.Environment.IsEnvironment("Test"))
        return;
    var databaseType = builder.Configuration.DatabaseType();
    var connectionString = builder.Configuration.ConnectionString();

    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    DatabaseMigration.Migrate(databaseType, connectionString, serviceScope.ServiceProvider);
}

public partial class Program {
    protected Program() { }
}