using Microsoft.AspNetCore.Mvc;

namespace Accountater.WebApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
