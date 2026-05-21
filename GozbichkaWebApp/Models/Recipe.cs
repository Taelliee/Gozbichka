using GozbichkaWebApp.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace GozbichkaWebApp.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CookingMethod { get; set; }
        public string ImageURL { get; set; }
        public double CookingTime { get; set; }
        public int Portions { get; set; }
        public double Rating { get; set; }

        public int MealCategoryId { get; set; }
        public MealCategory MealCategory { get; set; }

        public int DifficultyId { get; set; }
        public Difficulty Difficulty { get; set; }

        public ICollection<RecipeProduct> RecipeProducts { get; set; }
    }
}
