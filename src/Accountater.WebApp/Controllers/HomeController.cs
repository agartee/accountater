using Microsoft.AspNetCore.Mvc;

namespace Accountater.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
