public class RecipeFilters
{
    public string? Name { get; set; }
    public ICollection<long>? IngredientIds { get; set; }
    public ICollection<long>? DietaryTagIds { get; set; }
}
