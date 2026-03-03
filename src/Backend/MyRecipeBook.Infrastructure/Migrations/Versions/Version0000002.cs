using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_RECIPES, "Migration to create Recipes table")]
public class Version0000002 : VersionBase
{
    private const string RecipesTableName = "Recipes";

    public override void Up()
    {
        CreateTable(RecipesTableName)
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("CookingTime").AsInt32().Nullable()
            .WithColumn("Difficulty").AsInt32().Nullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey(
                "FK_Recipe_User_Id", "Users", "Id");

        CreateTable("Ingredients")
            .WithColumn("Item").AsString().NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey(
                "FK_Ingredient_Recipe_Id", RecipesTableName, "Id")
            .OnDelete(System.Data.Rule.Cascade);

        CreateTable("Instructions")
            .WithColumn("Step").AsInt32().NotNullable()
            .WithColumn("Text").AsString(2000).NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey(
                "FK_Instruction_Recipe_Id", RecipesTableName, "Id")
            .OnDelete(System.Data.Rule.Cascade);

        CreateTable("DishType")
            .WithColumn("Type").AsString(255).NotNullable()
            .WithColumn("RecipeId").AsInt64().NotNullable().ForeignKey(
                "FK_DishType_Recipe_Id", RecipesTableName, "Id")
            .OnDelete(System.Data.Rule.Cascade);
    }
}
