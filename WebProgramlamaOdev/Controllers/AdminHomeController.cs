using Microsoft.AspNetCore.Mvc;

namespace WebProgramlamaOdev.Controllers
{
    public class AdminHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
