using EnglishDictionaryWeb.Data;
using EnglishDictionaryWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishDictionaryWeb.Controllers
{
    [Authorize]
    public class PracticeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public PracticeController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Filter ve sıralama için parametre olarak sortOrder alıyoruz
        public async Task<IActionResult> PracticeList(string sortOrder)
        {
            var user = await _userManager.GetUserAsync(User);

            // Sadece Level > 0 olan kelimeleri çekiyoruz (öğrenilmemiş veya pratik yapılacaklar)
            var wordsQuery = _context.Words
                .Where(w => w.AppUserId == user.Id && w.Level > 0);

            // Sıralama seçeneğine göre sıralama yap
            switch (sortOrder)
            {
                case "level_desc":
                    wordsQuery = wordsQuery.OrderByDescending(w => w.Level);
                    break;
                case "level_asc":
                    wordsQuery = wordsQuery.OrderBy(w => w.Level);
                    break;
                case "text_asc":
                    wordsQuery = wordsQuery.OrderBy(w => w.Text);
                    break;
                case "text_desc":
                    wordsQuery = wordsQuery.OrderByDescending(w => w.Text);
                    break;
                default:
                    wordsQuery = wordsQuery.OrderBy(w => w.Text);
                    break;
            }

            var words = await wordsQuery.ToListAsync();

            ViewData["CurrentSort"] = sortOrder;
            TempData["CurrentSort"] = sortOrder;

            return View(words);
        }
        [HttpPost]
        public async Task<IActionResult> IncreaseLevel(int id)
        {
            var word = await _context.Words.FindAsync(id);
            if (word != null && word.AppUserId == _userManager.GetUserId(User) && word.Level < 7)
            {
                word.Level++;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("PracticeList", new { sortOrder = TempData["CurrentSort"] });
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseLevel(int id)
        {
            var word = await _context.Words.FindAsync(id);
            if (word != null && word.AppUserId == _userManager.GetUserId(User) && word.Level > 1)
            {
                word.Level--;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("PracticeList", new { sortOrder = TempData["CurrentSort"] });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var word = await _context.Words.FindAsync(id);
            if (word != null && word.AppUserId == _userManager.GetUserId(User))
            {
                _context.Words.Remove(word);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("PracticeList");
        }

    }
}
