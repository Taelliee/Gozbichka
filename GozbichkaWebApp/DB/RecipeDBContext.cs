using GozbichkaWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
namespace GozbichkaWebApp.DB
{
    public class RecipeDBContext : DbContext
    {
        public DbSet<Allergen> Allergens { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<MealCategory> MealCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAllergen> ProductAllergens { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeProduct> RecipeProducts { get; set; }
        public DbSet<RecipeSeasonalCategory> RecipeSeasonalCategories { get; set; }
        public DbSet<RecipeStep> RecipeSteps { get; set; }
        public DbSet<Refrigerator> Refrigerators { get; set; }
        public DbSet<SeasonalCategory> SeasonalCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAllergen> UserAllergens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-VLMAI0N;Database=Gozbichka;Trusted_Connection=True;TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite primary keys for many-to-many join tables
            modelBuilder.Entity<Favorites>()
                .HasKey(f => new { f.UserId, f.RecipeId });

            modelBuilder.Entity<History>()
                .HasKey(h => new { h.UserId, h.RecipeId, h.ViewAt });

            modelBuilder.Entity<ProductAllergen>()
                .HasKey(pa => new { pa.ProductId, pa.AllergenId });

            modelBuilder.Entity<RecipeProduct>()
                .HasKey(rp => new { rp.RecipeId, rp.ProductId });

            modelBuilder.Entity<RecipeProduct>()
                .HasOne(rp => rp.Product)
                .WithMany(p => p.RecipeProducts)
                .HasForeignKey(rp => rp.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete issues

            modelBuilder.Entity<RecipeProduct>()
                .HasOne(rp => rp.SubstituteProduct)
                .WithMany() // No navigation property back from Product for substitutes
                .HasForeignKey(rp => rp.SubstituteProductId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascade delete issues

            modelBuilder.Entity<RecipeSeasonalCategory>()
                .HasKey(rsc => new { rsc.RecipeId, rsc.SeasonalCategoryId });

            modelBuilder.Entity<Refrigerator>()
                .HasKey(r => new { r.UserId, r.ProductId });

            modelBuilder.Entity<UserAllergen>()
                .HasKey(ua => new { ua.UserId, ua.AllergenId });
        }
    }
}
