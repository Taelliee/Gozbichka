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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(RecipeDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["LoginError"] = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return RedirectToAction("Index", "Home");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == model.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                TempData["LoginError"] = "Невалидно потребителско име или парола.";
                return RedirectToAction("Index", "Home");
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserIcon", user.IconURL);
            HttpContext.Session.SetString("UserRole", user.Role);

            if (model.RememberMe)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true,
                    Secure = true
                };
                Response.Cookies.Append("RememberMe_UserId", user.UserId.ToString(), cookieOptions);
            }

            TempData["LoginSuccess"] = $"Добре дошли, {user.Name}!";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["RegisterError"] = string.Join(" ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return RedirectToAction("Index", "Home");
            }

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (existingUser != null)
            {
                TempData["RegisterError"] = "Този имейл вече е регистриран.";
                return RedirectToAction("Index", "Home");
            }

            var existingUserName = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == model.UserName);

            if (existingUserName != null)
            {
                TempData["RegisterError"] = "Това потребителско име вече е заето.";
                return RedirectToAction("Index", "Home");
            }

            var newUser = new User
            {
                Email = model.Email,
                Name = model.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            HttpContext.Session.SetString("UserName", newUser.Name);
            HttpContext.Session.SetString("UserIcon", newUser.IconURL);
            HttpContext.Session.SetString("UserRole", newUser.Role);

            TempData["RegisterSuccess"] = $"Добре дошли, {newUser.Name}! Регистрацията е успешна.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Response.Cookies.Delete("RememberMe_UserId");
            TempData["LogoutSuccess"] = "Успешно излязохте от системата.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile profileImage)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (userId == 0 || profileImage == null || profileImage.Length == 0)
            {
                TempData["LoginError"] = "Моля, изберете валидна снимка.";
                return RedirectToAction("Index", "Home");
            }

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profiles");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + profileImage.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(fileStream);
            }

            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IconURL = "/images/profiles/" + uniqueFileName;
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("UserIcon", user.IconURL);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}