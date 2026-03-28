namespace GozbichkaWebApp.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public ICollection<ProductAllergen> ProductAllergens { get; set; }
        public ICollection<RecipeProduct> RecipeProducts { get; set; }
        public ICollection<Refrigerator> Refrigerators { get; set; }
    }
}
