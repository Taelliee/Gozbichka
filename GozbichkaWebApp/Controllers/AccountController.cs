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
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["LoginError"] = "Моля, попълнете всички полета.";
                return RedirectToAction("Index", "Home");
            }

            // Намиране на потребител по имейл
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                TempData["LoginError"] = "Невалиден имейл или парола.";
                return RedirectToAction("Index", "Home");
            }

            if (user.Password != model.Password)
            {
                TempData["LoginError"] = "Невалидна парола.";
                return RedirectToAction("Index", "Home");
            }

            // Standard Session Login
            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserIcon", user.IconURL);
            HttpContext.Session.SetString("UserRole", user.Role);

            // --- REMEMBER ME LOGIC ---
            if (model.RememberMe)
            {
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true, // Protects from XSS attacks
                    Secure = true    // Ensures it's only sent over HTTPS
                };
                Response.Cookies.Append("RememberMe_UserId", user.UserId.ToString(), cookieOptions);
            }
            // -------------------------

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

            
            if (model.Password != model.ConfirmPassword)
            {
                TempData["RegisterError"] = "Паролите не съвпадат.";
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
                Password = model.Password 
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
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            // --- REMEMBER ME LOGIC (Clear the cookie on logout) ---
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

            // 1. Define where to save the image (wwwroot/images/profiles)
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "profiles");

            // Ensure the folder exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // 2. Create a unique file name (e.g., 5f4dcc3b5aa765d61d8327deb882cf99_myface.png)
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + profileImage.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // 3. Save the image to the physical folder
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(fileStream);
            }

            // 4. Update the user in the database
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.IconURL = "/images/profiles/" + uniqueFileName;
                await _context.SaveChangesAsync();

                // Update the session so the UI changes immediately
                HttpContext.Session.SetString("UserIcon", user.IconURL);
            }

            TempData["LoginSuccess"] = "Профилната снимка е обновена успешно!";
            return RedirectToAction("Index", "Home");
        }
    }
}