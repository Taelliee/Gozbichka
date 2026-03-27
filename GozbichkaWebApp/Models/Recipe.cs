using GozbichkaWebApp.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace GozbichkaWebApp.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public double CookingTime { get; set; }
        public int Portions { get; set; }

        public int MealCategory_ID { get; set; }
        public MealCategory MealCategory { get; set; }

        public DifficultyType difficulty { get; set; }

        //getimage?

        public ICollection<Rating> Ratings { get; set; }
        public ICollection<RecipeStep> RecipeSteps { get; set; }
<<<<<<< Updated upstream
        public ICollection<Product> Products { get; set; }
        public ICollection<RecipeProduct> RecipeProducts { get; set; }
        public ICollection<History> HistoryRecipes { get; set; }
        public ICollection<Favorites> FavoriteRecipes { get; set; }
        public ICollection<RecipeSeasonalCategory> RecipeSeasonalCategories { get; set; }
=======

>>>>>>> Stashed changes
    }
}
