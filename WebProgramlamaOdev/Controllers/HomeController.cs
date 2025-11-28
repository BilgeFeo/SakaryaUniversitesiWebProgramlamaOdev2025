using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.ModelDtos;




namespace WebProgramlamaOdev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public List<ApplicationUser> ApplicationUserListTemp = new List<ApplicationUser>();


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
        public IActionResult AccountCreationSuccesful(ApplicationUserDto applicationUserDto)
        {

            if(ModelState.IsValid)
            {
                  
                ApplicationUserListTemp.Add(new ApplicationUser(applicationUserDto));
                TempData["Name"] = ApplicationUserListTemp[0].FullName;
                return View();
            }

            else
            {
                ViewBag.GeneralError = "Hata: Lutfen asagidaki alanlardaki bilgileri kontrol edip tekrar deneyiniz.";
                return RedirectToAction("CreateAccount",applicationUserDto);
            }
            
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
