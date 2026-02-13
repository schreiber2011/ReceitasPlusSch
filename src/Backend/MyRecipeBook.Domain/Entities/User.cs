using System.ComponentModel.DataAnnotations.Schema;

namespace MyRecipeBook.Domain.Entities;

[Table("users")]
public class User : EntiyBase
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
