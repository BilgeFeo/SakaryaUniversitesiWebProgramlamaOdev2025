using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.DTOs;

namespace WebProgramlamaOdev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public static List<ApplicationUser> ApplicationUserListTemp = new List<ApplicationUser>();
        public static Dictionary<string, ApplicationUser> SavedUsers = new Dictionary<string, ApplicationUser>();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TriesAccountCreation(MemberRegisterRequestDto applicationUserDto)
        {

            if (ModelState.IsValid)
            {
                
                return View();
            }

            else
            {
                
                ViewBag.GeneralError = "Lutfen asagidaki alanları kontrol edip tekrar deneyiniz.";

                
                return View("CreateAccount", applicationUserDto);
            }
        }

        [HttpGet]
        public IActionResult RegistrationSuccess()
        {
            if (TempData["Name"] == null)
            {
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        [HttpPost]

        public IActionResult UserLoginAttempt(LoginRequestDto loginRequestDto)
        {



            if (ModelState.IsValid)
            {


                if (loginRequestDto.Email == "b231210033@sakarya.edu.tr" && loginRequestDto.Password == "sau")
                {

                    return RedirectToAction("Index","AdminHome");

                }
                
            }
            return RedirectToAction("Index");
        } 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
    }
}