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
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            List<int> fridgeIds = new List<int>();
            List<int> activeIds = new List<int>();

            if (userId > 0)
            {
                fridgeIds = _context.Refrigerators.Where(r => r.UserId == userId).Select(r => r.ProductId).ToList();
            }
            else
            {
                var guestFridgeCookie = Request.Cookies["GuestFridge"];
                if (!string.IsNullOrEmpty(guestFridgeCookie) && guestFridgeCookie != "NONE")
                    fridgeIds = guestFridgeCookie.Split(',').Select(int.Parse).ToList();
            }

            if (Request.Cookies.ContainsKey("ActiveSearch"))
            {
                var activeStateCookie = Request.Cookies["ActiveSearch"];

                if (!string.IsNullOrWhiteSpace(activeStateCookie) && activeStateCookie != "NONE")
                {
                    activeIds = activeStateCookie.Split(',').Select(int.Parse).ToList();
                    activeIds = activeIds.Where(id => fridgeIds.Contains(id)).ToList();
                }
                else
                {
                    activeIds = new List<int>();
                }
            }
            else
            {
                activeIds = fridgeIds.ToList();
            }

            var vm = new RefrigeratorViewModel
            {
                UserId = userId,
                AllProducts = _context.Products.ToList(),
                FridgeProductIds = fridgeIds,
                ActiveSearchProductIds = activeIds
            };

            return View("Refrigerator", vm);
        }

        [HttpPost]
        public IActionResult AutoSaveFridge(int userId, List<int> fridgeProductIds, List<int> activeSearchProductIds)
        {
            if (userId > 0)
            {
                var existing = _context.Refrigerators.Where(r => r.UserId == userId);
                _context.Refrigerators.RemoveRange(existing);

                if (fridgeProductIds != null)
                {
                    foreach (var id in fridgeProductIds)
                        _context.Refrigerators.Add(new Refrigerator { UserId = userId, ProductId = id });
                }
                _context.SaveChanges();
            }

            var cookieOptions = new CookieOptions { Expires = DateTime.Now.AddDays(30) };

            string fridgeCookieValue = (fridgeProductIds != null && fridgeProductIds.Any())
                ? string.Join(",", fridgeProductIds)
                : "NONE";

            string activeCookieValue = (activeSearchProductIds != null && activeSearchProductIds.Any())
                ? string.Join(",", activeSearchProductIds)
                : "NONE";

            Response.Cookies.Append("GuestFridge", fridgeCookieValue, cookieOptions);
            Response.Cookies.Append("ActiveSearch", activeCookieValue, cookieOptions);

            return Ok();
        }

        [HttpPost]
        public IActionResult SearchRecipes(RefrigeratorViewModel model)
        {
            model.UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            AutoSaveFridge(model.UserId, model.FridgeProductIds, model.ActiveSearchProductIds);

            if (model.ActiveSearchProductIds == null || !model.ActiveSearchProductIds.Any())
            {
                model.MatchingRecipes = new List<Recipe>();
            }
            else
            {
                model.MatchingRecipes = _context.Recipes
                    .Include(r => r.RecipeProducts)
                    .Where(r => r.RecipeProducts.Any(rp => model.ActiveSearchProductIds.Contains(rp.ProductId)))
                    .ToList();
            }

            model.AllProducts = _context.Products.ToList();
            return View("Refrigerator", model);
        }
    }
}