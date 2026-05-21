using GozbichkaWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace GozbichkaWebApp.DB
{
    public class RecipeDBContext : DbContext
    {
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<MealCategory> MealCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeProduct> RecipeProducts { get; set; }
        public DbSet<Refrigerator> Refrigerators { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-VLMAI0N;Database=Gozbichka;Trusted_Connection=True;TrustServerCertificate=True;");
            //optionsBuilder.UseSqlServer("Server=PETIAIGNATOVA\\SQLEXPRESS;Database=Gozbichka;Trusted_Connection=True;TrustServerCertificate=True;");
            //optionsBuilder.UseSqlServer("Server=JUSTFREX\\SQLEXPRESS;Database=Gozbichka;Trusted_Connection=True;TrustServerCertificate=True;");
            //optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Gozbichka;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RecipeProduct>()
                .HasKey(rp => new { rp.RecipeId, rp.ProductId });

            modelBuilder.Entity<RecipeProduct>()
                .HasOne(rp => rp.Product)
                .WithMany(p => p.RecipeProducts)
                .HasForeignKey(rp => rp.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RecipeProduct>()
                .HasOne(rp => rp.SubstituteProduct)
                .WithMany() 
                .HasForeignKey(rp => rp.SubstituteProductId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Refrigerator>()
                .HasKey(r => new { r.UserId, r.ProductId });

            modelBuilder.Entity<MealCategory>().HasData(
                new MealCategory { MealCategoryId = 1, Name = "Основни" },
                new MealCategory { MealCategoryId = 2, Name = "Супи" },
                new MealCategory { MealCategoryId = 3, Name = "Салати" },
                new MealCategory { MealCategoryId = 4, Name = "Десерти" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, ProductName = "Хляб" },
                new Product { ProductId = 2, ProductName = "Мляко" },
                new Product { ProductId = 3, ProductName = "Сирене" },
                new Product { ProductId = 4, ProductName = "Кашкавал" },
                new Product { ProductId = 5, ProductName = "Яйца" },
                new Product { ProductId = 6, ProductName = "Масло" },
                new Product { ProductId = 7, ProductName = "Кисело мляко" },
                new Product { ProductId = 8, ProductName = "Пилешко месо" },
                new Product { ProductId = 9, ProductName = "Свинско месо" },
                new Product { ProductId = 10, ProductName = "Телешко месо" },
                new Product { ProductId = 11, ProductName = "Ориз" },
                new Product { ProductId = 12, ProductName = "Макарони" },
                new Product { ProductId = 13, ProductName = "Спагети" },
                new Product { ProductId = 14, ProductName = "Домати" },
                new Product { ProductId = 15, ProductName = "Краставици" },
                new Product { ProductId = 16, ProductName = "Картофи" },
                new Product { ProductId = 17, ProductName = "Лук" },
                new Product { ProductId = 18, ProductName = "Чесън" },
                new Product { ProductId = 19, ProductName = "Моркови" },
                new Product { ProductId = 20, ProductName = "Чушки" },
                new Product { ProductId = 21, ProductName = "Ябълки" },
                new Product { ProductId = 22, ProductName = "Банани" },
                new Product { ProductId = 23, ProductName = "Портокали" },
                new Product { ProductId = 24, ProductName = "Лимони" },
                new Product { ProductId = 25, ProductName = "Захар" },
                new Product { ProductId = 26, ProductName = "Сол" },
                new Product { ProductId = 27, ProductName = "Брашно" },
                new Product { ProductId = 28, ProductName = "Олио" },
                new Product { ProductId = 29, ProductName = "Мед" },
                new Product { ProductId = 30, ProductName = "Шоколад" }
            );

            modelBuilder.Entity<Difficulty>().HasData(
                new Difficulty { DifficultyId = 1, DifficultyType = "Лесно" },
                new Difficulty { DifficultyId = 2, DifficultyType = "Средно" },
                new Difficulty { DifficultyId = 3, DifficultyType = "Трудно" }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Name = "admin",
                    Email = "admin@admin.com",
                    Password = "$2a$11$v/tnIAFbOncFT786KQVyNOpmoIkRmBB9SiqkzFYrDfeIGrGpUpf9i", //admin
                    Role = "Admin",
                    IconURL = "/images/default-user-icon.png"
                }
            );

            modelBuilder.Entity<Recipe>().HasData(
                new Recipe
                {
                    RecipeId = 1,
                    Title = "Мусака",
                    Description = "Класическа мусака.",
                    CookingMethod = "Подредете продуктите и изпечете.",
                    ImageURL = "/images/Moussaka.webp",
                    CookingTime = 90,
                    Portions = 4,
                    Rating = 0,
                    MealCategoryId = 1,
                    DifficultyId = 2
                },
                new Recipe
                {
                    RecipeId = 2,
                    Title = "Яйчена салата",
                    Description = "Лека яйчена салата.",
                    CookingMethod = "Сварете яйцата, смесете продуктите и сервирайте.",
                    ImageURL = "/images/qichena.jpg",
                    CookingTime = 15,
                    Portions = 2,
                    Rating = 0,
                    MealCategoryId = 3,
                    DifficultyId = 1
                },
                new Recipe
                {
                    RecipeId = 3,
                    Title = "Баница",
                    Description = "Семпла домашна баница.",
                    CookingMethod = "Смесете продуктите, подредете и изпечете.",
                    ImageURL = "/images/Banitsa.jpg",
                    CookingTime = 45,
                    Portions = 6,
                    Rating = 0,
                    MealCategoryId = 1,
                    DifficultyId = 2
                },
                new Recipe
                {
                    RecipeId = 4,
                    Title = "Крем Брюле",
                    Description = "Класически крем брюле.",
                    CookingMethod = "Смесете продуктите, изпечете леко и охладете.",
                    ImageURL = "/images/Creme_Brulee.jpg",
                    CookingTime = 50,
                    Portions = 4,
                    Rating = 0,
                    MealCategoryId = 4,
                    DifficultyId = 2
                }
            );

            modelBuilder.Entity<RecipeProduct>().HasData(
                new RecipeProduct { RecipeId = 1, ProductId = 16, Quantity = 5, Unit = "бр.", isOptional = false, SubstituteProductId = null },
                new RecipeProduct { RecipeId = 1, ProductId = 9, Quantity = 500, Unit = "г", isOptional = false, SubstituteProductId = null },
                new RecipeProduct { RecipeId = 1, ProductId = 17, Quantity = 1, Unit = "бр.", isOptional = false, SubstituteProductId = null },
                new RecipeProduct { RecipeId = 1, ProductId = 5, Quantity = 2, Unit = "бр.", isOptional = false, SubstituteProductId = null },
                new RecipeProduct { RecipeId = 1, ProductId = 2, Quantity = 200, Unit = "мл", isOptional = false, SubstituteProductId = null },
                new RecipeProduct { RecipeId = 1, ProductId = 26, Quantity = 1, Unit = "ч.л.", isOptional = false, SubstituteProductId = null },
                new RecipeProduct { RecipeId = 1, ProductId = 28, Quantity = 2, Unit = "с.л.", isOptional = false, SubstituteProductId = null },

                new RecipeProduct { RecipeId = 2, ProductId = 5, Quantity = 4, Unit = "бр.", isOptional = false, SubstituteProductId = null },
                new RecipeProduct { RecipeId = 2, ProductId = 7, Quantity = 200, Unit = "г", isOptional = false, SubstituteProductId = null },
                new RecipeProduct { RecipeId = 2, ProductId = 18, Quantity = 1, Unit = "скилидка", isOptional = true, SubstituteProductId = null },
                new RecipeProduct { RecipeId = 2, ProductId = 26, Quantity = 1, Unit = "щипка", isOptional = false, SubstituteProductId = null },

                new RecipeProduct { RecipeId = 3, ProductId = 27, Quantity = 300, Unit = "г", isOptional = false, SubstituteProductId = null },
                new RecipeProduct { RecipeId = 3, ProductId = 5, Quantity = 4, Unit = "бр.", isOptional = false, SubstituteProductId = null },
                new RecipeProduct { RecipeId = 3, ProductId = 3, Quantity = 250, Unit = "г", isOptional = false, SubstituteProductId = null },
                new RecipeProduct { RecipeId = 3, ProductId = 6, Quantity = 100, Unit = "г", isOptional = false, SubstituteProductId = null },
                new RecipeProduct { RecipeId = 3, ProductId = 2, Quantity = 150, Unit = "мл", isOptional = false, SubstituteProductId = null },

                new RecipeProduct { RecipeId = 4, ProductId = 2, Quantity = 500, Unit = "мл", isOptional = false, SubstituteProductId = null },
                new RecipeProduct { RecipeId = 4, ProductId = 5, Quantity = 4, Unit = "бр.", isOptional = false, SubstituteProductId = null },
                new RecipeProduct { RecipeId = 4, ProductId = 25, Quantity = 100, Unit = "г", isOptional = false, SubstituteProductId = null },
                new RecipeProduct { RecipeId = 4, ProductId = 30, Quantity = 50, Unit = "г", isOptional = true, SubstituteProductId = null }
            );
        }
    }
}
