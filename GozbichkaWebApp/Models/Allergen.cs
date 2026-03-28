namespace GozbichkaWebApp.Models
{
    public class Allergen
    {
        public int AllergenId { get; set; }
        public string AllergenName {  get; set; }

        public ICollection<ProductAllergen> ProductAllergens { get; set; }
        public ICollection<UserAllergen> UserAllergens { get; set; }
    }
}
