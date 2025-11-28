using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.ModelDtos;


namespace WebProgramlamaOdev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public string AccountCreationSuccesful(ApplicaitonUserDto applicationUserDto)
        {

            if(ModelState.IsValid)
            {
                return applicationUserDto.FirstName + " " +applicationUserDto.LastName+" "+ applicationUserDto.Email+" "+applicationUserDto.Password+" "+ applicationUserDto.Phone;
            }

            else
            {
                return "Hata";
            }

        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
