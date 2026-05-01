using GozbichkaWebApp.DB;
using GozbichkaWebApp.Models;
using GozbichkaWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GozbichkaWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly RecipeDBContext _context;

        public AccountController(RecipeDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["LoginError"] = "Моля, попълнете всички полета.";
                return RedirectToAction("Index", "Home");
            }

            // Find user by email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                TempData["LoginError"] = "Невалиден имейл или парола.";
                return RedirectToAction("Index", "Home");
            }

            // TODO: Add password hashing verification
            // For now, simple comparison (you should hash passwords in production!)
            if (user.Password != model.Password)
            {
                TempData["LoginError"] = "Невалидна парола.";
                return RedirectToAction("Index", "Home");
            }

            // Store user info in session
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.Name);

            TempData["LoginSuccess"] = $"Добре дошли, {user.Name}!";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["RegisterError"] = "Моля, попълнете всички полета.";
                return RedirectToAction("Index", "Home");
            }

            // Check if passwords match
            if (model.Password != model.ConfirmPassword)
            {
                TempData["RegisterError"] = "Паролите не съвпадат.";
                return RedirectToAction("Index", "Home");
            }

            // Check if email already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (existingUser != null)
            {
                TempData["RegisterError"] = "Този имейл вече е регистриран.";
                return RedirectToAction("Index", "Home");
            }

            // Check if username already exists
            var existingUserName = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == model.UserName);

            if (existingUserName != null)
            {
                TempData["RegisterError"] = "Това потребителско име вече е заето.";
                return RedirectToAction("Index", "Home");
            }

            // Create new user
            // TODO: Add password hashing in production!
            var newUser = new User
            {
                Email = model.Email,
                Name = model.UserName,
                Password = model.Password // In production, hash this!
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Automatically log in the user
            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            HttpContext.Session.SetString("UserName", newUser.Name);

            TempData["RegisterSuccess"] = $"Добре дошли, {newUser.Name}! Регистрацията е успешна.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["LogoutSuccess"] = "Успешно излязохте от системата.";
            return RedirectToAction("Index", "Home");
        }
    }
}