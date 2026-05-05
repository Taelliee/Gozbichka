namespace GozbichkaWebApp.Models
{
    public class Difficulty
    {
        public int DifficultyId { get; set; }
        public string DifficultyType { get; set; }
        public ICollection<Recipe> Recipes { get; set; }
    }
}
