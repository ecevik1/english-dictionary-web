using EnglishDictionaryWeb.Data;
using EnglishDictionaryWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishDictionaryWeb.Controllers
{
    [Authorize] // Sadece giriş yapan kullanıcılar erişebilir
    public class WordController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public WordController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var words = await _context.Words
                .Where(w => w.AppUserId == user.Id)
                .ToListAsync();

            return View(words);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(string text, string translation)
        {
            var user = await _userManager.GetUserAsync(User);

            var word = new Word
            {
                Text = text,
                Translation = translation,
                AppUserId = user.Id
            };

            _context.Words.Add(word);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
