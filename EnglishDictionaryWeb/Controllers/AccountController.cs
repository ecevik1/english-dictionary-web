using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EnglishDictionaryWeb.Models;

namespace EnglishDictionaryWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register() => View();
        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password, string repeatPassword)
        {
            if (password != repeatPassword)
            {
                ModelState.AddModelError(string.Empty, "Şifreler uyuşmuyor.");
                return View();
            }

            var user = new AppUser { UserName = username, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            // Hataları modele yansıt
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }


        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError(string.Empty, "Giriş başarısız. Kullanıcı adı veya şifre hatalı.");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
