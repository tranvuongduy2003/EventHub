namespace EventHub.Domain.Models.Category;

public class CategoryModel
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string IconImage { get; set; }

    public string Color { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}