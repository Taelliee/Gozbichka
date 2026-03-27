using GozbichkaWebApp.Enums;

namespace GozbichkaWebApp.Models
{
    public class Difficulty
    {
        public int DifficultyId { get; set; }
        public DifficultyType DifficultyType { get; set; }

        public ICollection<Recipe> Recipes { get; set; }
    }
}
