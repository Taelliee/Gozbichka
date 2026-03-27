namespace GozbichkaWebApp.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public ICollection<ProductAllergens> ProductAllergens { get; set; }
        public ICollection<RecipeProduct> recipeProducts { get; set; }
    }
}
