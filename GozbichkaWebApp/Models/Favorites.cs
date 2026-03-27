using GozbichkaWebApp.Enums;

namespace GozbichkaWebApp.Models
{
    public class Favorites
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int UserId { get; set; }    
        public User User { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
