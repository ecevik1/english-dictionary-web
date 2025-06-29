using EnglishDictionaryWeb.Data;
using EnglishDictionaryWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishDictionaryWeb.Controllers
{
    [Authorize]
    public class LearnedController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LearnedController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        

        

        // Öğrenilen Kelimeler (Level = 0)
        public async Task<IActionResult> LearnedList()
        {
            var user = await _userManager.GetUserAsync(User);

            var learnedWords = await _context.Words
                .Where(w => w.AppUserId == user.Id && w.Level == 0)
                .ToListAsync();

            return View(learnedWords);
        }
    }
}
