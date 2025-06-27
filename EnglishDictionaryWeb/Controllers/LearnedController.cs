using Microsoft.AspNetCore.Mvc;

namespace EnglishDictionaryWeb.Controllers
{
    public class LearnedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
