namespace GozbichkaWebApp.Models
{
    public class UserAllergen
    {
        public int AllergenId { get; set; }
        public Allergens Allergens { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
