namespace GozbichkaWebApp.Models
{
    public class Refrigerator
    {
        public bool IsSelected { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
