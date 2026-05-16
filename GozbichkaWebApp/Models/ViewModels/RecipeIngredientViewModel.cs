namespace GozbichkaWebApp.Models.ViewModels
{
    public class RecipeIngredientViewModel
    {
        public string ProductName { get; set; } = string.Empty;
        public double Quantity { get; set; }
        public string QuantityText { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public bool IsOptional { get; set; }
    }
}
