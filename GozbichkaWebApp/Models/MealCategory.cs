namespace GozbichkaWebApp.Models
{
    public class MealCategory
    {
        public int MealCategoryId { get; set; }
        public string Name { get; set; }

        ICollection<Recipe> Recipes { get; set; }
    }
}
