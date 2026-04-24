using GozbichkaWebApp.Enums;

namespace GozbichkaWebApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        //getimage?
        public string IconURL { get; set; }

        //more changes to password
        public string Password { get; set; }

        //public ICollection<UserAllergen> UserAllergens { get; set; }
        public ICollection<Refrigerator> Refrigerators { get; set; }
        //public ICollection<Favorites> Favorites { get; set; }
        //public ICollection<History> Histories { get; set; }
        //public ICollection<Rating> Ratings { get; set; }
    }
}
