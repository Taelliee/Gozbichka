namespace GozbichkaWebApp.Models
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int Score { get; set; } // 1 to 5 stars
        public string Comment { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }


    }
}
