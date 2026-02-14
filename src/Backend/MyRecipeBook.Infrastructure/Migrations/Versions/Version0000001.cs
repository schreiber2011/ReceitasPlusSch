using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(1, "Initial migration to create Users table")]
public class Version0000001 : VersionBase
{
    public override void Up()
    {
        CreateTable("Users")
                .WithColumn("Name").AsString(255).NotNullable()
                .WithColumn("Email").AsString(255).NotNullable()
                .WithColumn("Password").AsString(255).NotNullable();
    }
}
