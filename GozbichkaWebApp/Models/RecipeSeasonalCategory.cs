namespace GozbichkaWebApp.Models
{
    public class RecipeSeasonalCategory
    {
        public int SeasonalCategoryId { get; set; }
        public SeasonalCategory SeasonalCategory { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
