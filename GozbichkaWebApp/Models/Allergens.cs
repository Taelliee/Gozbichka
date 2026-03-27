namespace GozbichkaWebApp.Models
{
    public class Allergens
    {
        public int AllergenId { get; set; }
        public int AllergenName {  get; set; }

        public ICollection<ProductAllergens> ProductAllergens { get; set; }
        public ICollection<UserAllergen> UserAllergens { get; set; }
    }
}
