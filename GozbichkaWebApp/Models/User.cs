using GozbichkaWebApp.Enums;

namespace GozbichkaWebApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string IconURL { get; set; } = "/images/default-user-icon.png";
        public string Password { get; set; }
        public string Role { get; set; } = "User";

        public ICollection<Refrigerator> Refrigerators { get; set; }
    }
}
