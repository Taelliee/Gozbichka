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

        public Difficulty difficulty { get; set; }

        //getimage?

        public ICollection<Rating> Ratings { get; set; }
        public ICollection<RecipeStep> RecipeSteps { get; set; }

    }
}
