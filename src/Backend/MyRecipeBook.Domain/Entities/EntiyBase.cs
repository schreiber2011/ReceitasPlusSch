namespace MyRecipeBook.Domain.Entities;

public class EntiyBase
{
    public long Id { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

}
