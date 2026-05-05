using GozbichkaWebApp.DB;
using GozbichkaWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GozbichkaWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly RecipeDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // 1. Inject both the Database Context and the Web Host Environment (for images)
        public AdminController(RecipeDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Dashboard()
        {
            // Security Check
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                TempData["LoginError"] = "Нямате права за достъп до тази страница.";
                return RedirectToAction("Index", "Home");
            }

            // Fetch the data for the dropdowns
            ViewBag.Categories = _context.MealCategories.ToList();
            ViewBag.Difficulties = _context.Difficulties.ToList();
            ViewBag.Products = _context.Products.ToList();

            ViewBag.AllRecipes = _context.Recipes.Include(r => r.MealCategory).Include(r => r.Difficulty).ToList();

            return View();
        }

        // 2. The saving logic! Notice how ASP.NET Core automatically builds the List<RecipeProduct> for us!
        [HttpPost]
        public async Task<IActionResult> AddRecipe(
            string Title,
            int MealCategoryId,
            int DifficultyId,
            int CookTimeHours,
            int CookTimeMinutes,
            int Portions,
            string Description,
            string Method,
            IFormFile Image,
            List<RecipeProduct> RecipeProducts) // <-- The magic happens here
        {
            // Security Check (Just in case someone tries to POST without being an admin)
            if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Index", "Home");

            // --- IMAGE UPLOAD LOGIC ---
            string imagePath = "/images/placeholder-recipe.jpg"; // Default fallback image

            if (Image != null && Image.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "recipes");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }

                imagePath = "/images/recipes/" + uniqueFileName;
            }
            // --------------------------

            // Combine hours and minutes into total minutes for the database
            double totalCookingTimeMinutes = (CookTimeHours * 60) + CookTimeMinutes;

            // --- BUILD THE RECIPE ---
            var newRecipe = new Recipe
            {
                Title = Title,
                Description = Description,
                CookingMethod = Method,
                ImageURL = imagePath,
                CookingTime = totalCookingTimeMinutes,
                Portions = Portions,
                Rating = 0, // Starts at 0 for a new recipe
                MealCategoryId = MealCategoryId,
                DifficultyId = DifficultyId,
                RecipeProducts = new List<RecipeProduct>()
            };

            // --- ADD THE INGREDIENTS ---
            if (RecipeProducts != null && RecipeProducts.Any())
            {
                foreach (var product in RecipeProducts)
                {
                    newRecipe.RecipeProducts.Add(new RecipeProduct
                    {
                        ProductId = product.ProductId,
                        Quantity = product.Quantity,
                        Unit = product.Unit,
                        isOptional = false // Assuming all added this way are mandatory by default
                    });
                }
            }

            // --- SAVE TO DATABASE ---
            _context.Recipes.Add(newRecipe);
            await _context.SaveChangesAsync();

            TempData["LoginSuccess"] = $"Рецептата '{Title}' беше добавена успешно!";
            return RedirectToAction("Dashboard");
        }

        // 1. GET: Opens the pre-filled Edit page
        [HttpGet]
        public async Task<IActionResult> EditRecipe(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Index", "Home");

            // Fetch the recipe AND its included products
            var recipe = await _context.Recipes
                .Include(r => r.RecipeProducts)
                .ThenInclude(rp => rp.Product)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            if (recipe == null) return NotFound();

            ViewBag.Categories = _context.MealCategories.ToList();
            ViewBag.Difficulties = _context.Difficulties.ToList();
            ViewBag.Products = _context.Products.ToList();

            return View(recipe);
        }

        // 2. POST: Saves the edited changes
        [HttpPost]
        public async Task<IActionResult> EditRecipe(
            int RecipeId, string Title, int MealCategoryId, int DifficultyId,
            int CookTimeHours, int CookTimeMinutes, int Portions,
            string Description, string Method, IFormFile Image,
            string ExistingImageURL, List<RecipeProduct> RecipeProducts)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Index", "Home");

            var recipe = await _context.Recipes.Include(r => r.RecipeProducts).FirstOrDefaultAsync(r => r.RecipeId == RecipeId);
            if (recipe == null) return NotFound();

            // Update basic info
            recipe.Title = Title;
            recipe.MealCategoryId = MealCategoryId;
            recipe.DifficultyId = DifficultyId;
            recipe.CookingTime = (CookTimeHours * 60) + CookTimeMinutes;
            recipe.Portions = Portions;
            recipe.Description = Description;
            recipe.CookingMethod = Method;

            // Handle Image (Only replace if a new one was uploaded)
            if (Image != null && Image.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "recipes");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create)) { await Image.CopyToAsync(fileStream); }
                recipe.ImageURL = "/images/recipes/" + uniqueFileName;
            }
            else
            {
                recipe.ImageURL = ExistingImageURL;
            }

            // Handle Products: Remove old ones, add the new/edited ones
            var existingProducts = _context.Set<RecipeProduct>().Where(rp => rp.RecipeId == RecipeId);
            _context.Set<RecipeProduct>().RemoveRange(existingProducts);

            if (RecipeProducts != null && RecipeProducts.Any())
            {
                foreach (var product in RecipeProducts)
                {
                    recipe.RecipeProducts.Add(new RecipeProduct
                    {
                        ProductId = product.ProductId,
                        Quantity = product.Quantity,
                        Unit = product.Unit,
                        isOptional = false
                    });
                }
            }

            await _context.SaveChangesAsync();
            TempData["LoginSuccess"] = $"Рецептата '{Title}' беше обновена успешно!";
            return RedirectToAction("Dashboard");
        }

        // 3. POST: Deletes the recipe
        [HttpPost]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin") return RedirectToAction("Index", "Home");

            // Find the recipe in the database
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe != null)
            {
                // Entity Framework will automatically delete the related RecipeProducts (ingredients) 
                // because they are linked to this RecipeId!
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();

                TempData["LoginSuccess"] = $"Рецептата '{recipe.Title}' беше изтрита успешно!";
            }
            else
            {
                TempData["LoginError"] = "Възникна грешка: Рецептата не беше намерена.";
            }

            return RedirectToAction("Dashboard");
        }
    }
}