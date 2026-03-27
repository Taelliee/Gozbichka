namespace GozbichkaWebApp.Models
{
    public class History
    {
        public DateTime ViewAt { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Recipe> Recipes { get; set; }

    }
}
