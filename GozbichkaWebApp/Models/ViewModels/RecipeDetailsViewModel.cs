namespace GozbichkaWebApp.Models.ViewModels
{
    public class RecipeDetailsViewModel
    {
        public int RecipeId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Portions { get; set; }
        public double Rating { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string DifficultyName { get; set; } = string.Empty;
        public string CookingTimeText { get; set; } = string.Empty;
        public List<RecipeIngredientViewModel> Ingredients { get; set; } = new List<RecipeIngredientViewModel>();
        public List<RecipeStepViewModel> Steps { get; set; } = new List<RecipeStepViewModel>();
    }
}