namespace GozbichkaWebApp.Models
{
    public class SeasonalCategory
    {
        public int SeasonalCategoryId { get; set; }
        public int Name { get; set; }

        public ICollection<RecipeSeasonalCategory> RecipeSeasonalCategories { get; set; }
    }
}
