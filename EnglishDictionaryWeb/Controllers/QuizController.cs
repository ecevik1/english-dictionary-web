using Microsoft.AspNetCore.Mvc;

namespace EnglishDictionaryWeb.Controllers
{
    public class QuizController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
