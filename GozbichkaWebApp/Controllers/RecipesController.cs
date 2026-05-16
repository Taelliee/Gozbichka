using GozbichkaWebApp.DB;
using GozbichkaWebApp.Models;
using GozbichkaWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GozbichkaWebApp.Controllers
{
    public class RecipesController : Controller
    {
        private readonly RecipeDBContext _context;

        public RecipesController(RecipeDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> RecipeDetails(int id)
        {
            Recipe? recipe = await _context.Recipes
                .Include(r => r.MealCategory)
                .Include(r => r.Difficulty)
                .Include(r => r.RecipeProducts)
                    .ThenInclude(rp => rp.Product)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            if (recipe == null)
            {
                return NotFound();
            }

            List<string> steps = BuildSteps(recipe);

            RecipeDetailsViewModel model = new RecipeDetailsViewModel
            {
                RecipeId = recipe.RecipeId,
                Title = recipe.Title,
                Description = recipe.Description ?? string.Empty,
                ImageUrl = recipe.ImageURL ?? string.Empty,
                Portions = recipe.Portions,
                Rating = recipe.Rating,
                CategoryName = recipe.MealCategory?.Name ?? "Без категория",
                DifficultyName = recipe.Difficulty?.DifficultyType ?? "Не е зададена",
                CookingTimeText = FormatCookingTime(recipe.CookingTime),
                Ingredients = recipe.RecipeProducts
                    .Where(rp => rp.Product != null)
                    .OrderBy(rp => rp.Product.ProductName)
                    .Select(rp => new RecipeIngredientViewModel
                    {
                        ProductName = rp.Product.ProductName,
                        Quantity = rp.Quantity,
                        QuantityText = FormatQuantity(rp.Quantity),
                        Unit = rp.Unit ?? string.Empty,
                        IsOptional = rp.isOptional
                    })
                    .ToList(),
                Steps = steps
                    .Select((step, index) => new RecipeStepViewModel
                    {
                        Number = index + 1,
                        Title = $"Стъпка {index + 1}",
                        Description = step
                    })
                    .ToList()
            };

            return View(model);
        }

        private static List<string> BuildSteps(Recipe recipe)
        {
            List<string> steps = new List<string>
            {
                "Пригответе продуктите."
            };

            if (!string.IsNullOrWhiteSpace(recipe.CookingMethod))
            {
                List<string> methodSteps = recipe.CookingMethod
                    .Replace("\r\n", "\n")
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .ToList();

                if (methodSteps.Count <= 1)
                {
                    methodSteps = Regex.Split(recipe.CookingMethod.Trim(), @"(?<=[.!?])\s+")
                        .Where(step => !string.IsNullOrWhiteSpace(step))
                        .Select(step => step.Trim())
                        .ToList();
                }

                foreach (string methodStep in methodSteps)
                {
                    if (!steps.Any(step => string.Equals(step, methodStep, StringComparison.OrdinalIgnoreCase)))
                    {
                        steps.Add(methodStep);
                    }
                }
            }

            if (steps.Count == 0)
            {
                steps.Add("Подробни стъпки за рецептата ще бъдат добавени скоро.");
            }

            return steps;
        }

        private static string FormatQuantity(double quantity)
        {
            if (Math.Abs(quantity % 1) < 0.001)
            {
                return quantity.ToString("0", CultureInfo.InvariantCulture);
            }

            return quantity.ToString("0.##", CultureInfo.InvariantCulture);
        }

        private static string FormatCookingTime(double cookingTime)
        {
            if (cookingTime <= 0)
            {
                return "—";
            }

            if (cookingTime < 60)
            {
                return $"{cookingTime.ToString("0", CultureInfo.InvariantCulture)} мин.";
            }

            int hours = (int)(cookingTime / 60);
            int minutes = (int)(cookingTime % 60);

            if (minutes == 0)
            {
                return $"{hours} ч.";
            }

            return $"{hours} ч. {minutes} мин.";
        }
    }
}