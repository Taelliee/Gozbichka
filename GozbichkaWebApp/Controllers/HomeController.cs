using GozbichkaWebApp.DB;
using GozbichkaWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GozbichkaWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly RecipeDBContext _context;
        public HomeController(RecipeDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // --- AUTO-LOGIN LOGIC ---
            // If they are NOT logged into the session, check if they have the remember cookie
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                var rememberCookie = Request.Cookies["RememberMe_UserId"];

                if (!string.IsNullOrEmpty(rememberCookie) && int.TryParse(rememberCookie, out int userId))
                {
                    // Find the user in the database
                    var user = _context.Users.Find(userId);

                    if (user != null)
                    {
                        // Restore their session!
                        HttpContext.Session.SetInt32("UserId", user.UserId);
                        HttpContext.Session.SetString("UserName", user.Name);
                        HttpContext.Session.SetString("UserIcon", user.IconURL);
                        HttpContext.Session.SetString("UserRole", user.Role);
                    }
                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
