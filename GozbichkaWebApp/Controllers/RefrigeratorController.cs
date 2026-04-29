using GozbichkaWebApp.DB;
using GozbichkaWebApp.Models;
using GozbichkaWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GozbichkaWebApp.Controllers
{
    public class RefrigeratorController : Controller
    {
        private RecipeDBContext _context;

        public RefrigeratorController(RecipeDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int userId = _context.Users.Select(u => u.UserId).FirstOrDefault();

            var vm = new RefrigeratorViewModel
            {
                UserId = userId,
                AllProducts = _context.Products.ToList(),
                SelectedProductIds = _context.Refrigerators
                    .Where(r => r.UserId == userId)
                    .Select(r => r.ProductId)
                    .ToList()
            };

            return View("Refrigerator", vm);
        }

        [HttpPost]
        public IActionResult Update(RefrigeratorViewModel model)
        {
            int userId = model.UserId;

            var existing = _context.Refrigerators.Where(r => r.UserId == userId);
            _context.Refrigerators.RemoveRange(existing);

            foreach (var productId in model.SelectedProductIds)
            {
                _context.Refrigerators.Add(new Refrigerator
                {
                    UserId = userId,
                    ProductId = productId
                });
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SearchRecipes(RefrigeratorViewModel model)
        {
            if (model.SelectedProductIds == null || !model.SelectedProductIds.Any())
            {
                model.MatchingRecipes = new List<Recipe>();
            }
            else
            {
                model.MatchingRecipes = _context.Recipes
                    .Include(r => r.RecipeProducts)
                    .Where(r => r.RecipeProducts
                        .Any(rp => model.SelectedProductIds.Contains(rp.ProductId)))
                    .ToList();
            }

            model.AllProducts = _context.Products.ToList();

            return View("Refrigerator", model);
        }
    }
}
