using Microsoft.AspNetCore.Mvc;

namespace EnglishDictionaryWeb.Controllers
{
    public class WordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
