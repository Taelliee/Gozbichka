namespace GozbichkaWebApp.Models.ViewModels
{
    public class RefrigeratorViewModel
    {
        public int UserId { get; set; }

        public List<int> SelectedProductIds { get; set; } = new List<int>();

        // Всички продукти от базата
        public List<Product> AllProducts { get; set; } = new List<Product>();

        public List<Recipe> MatchingRecipes { get; set; } = new List<Recipe>();
    }
}
