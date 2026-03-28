namespace GozbichkaWebApp.Models
{
    public class MealCategory
    {
        public int MealCategoryId { get; set; }
        public string Name { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
    }
}
