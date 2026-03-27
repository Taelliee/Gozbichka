namespace GozbichkaWebApp.Models
{
    public class RecipeProduct
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public bool isOptional { get; set; }
        public int SubstituteProductId { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
