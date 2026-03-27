namespace GozbichkaWebApp.Models
{
    public class ProductAllergens
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int AllergenId { get; set; }
        public Allergens Allergens { get; set; }

        public ICollection<Product> ProductsCollection { get; set; }
        public ICollection<Allergens> AllergensCollection { get; set; }
    }
}
