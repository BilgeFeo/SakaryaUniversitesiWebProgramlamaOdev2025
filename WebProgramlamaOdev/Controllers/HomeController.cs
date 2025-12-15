using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.DTOs;
using WebProgramlamaOdev.Services;
using WebProgramlamaOdev.Services.ServiceInterfaces;

namespace WebProgramlamaOdev.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public static List<ApplicationUser> ApplicationUserListTemp = new List<ApplicationUser>();
        public static Dictionary<string, ApplicationUser> SavedUsers = new Dictionary<string, ApplicationUser>();


        public readonly IAuthService _authService;
        public readonly ISignUpService _signupService;
        

        public HomeController(ILogger<HomeController> logger,IAuthService authService, ISignUpService signUpService)
        {
            _logger = logger;
            _authService = authService;
            _signupService = signUpService;
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
        public async Task<IActionResult> TriesAccountCreation(MemberRegisterRequestDto applicationUserDto)
        {

            if (ModelState.IsValid)
            {
                if(await _signupService.RegisterMemberAsync(applicationUserDto))
                {
                    return RedirectToAction("Index");
                }

            }

            
             ViewBag.GeneralError = "Lutfen asagidaki alanları kontrol edip tekrar deneyiniz.";

                
             return View("CreateAccount", applicationUserDto);
            
        }

        
        [HttpPost]

        public async Task<IActionResult> UserLoginAttempt(LoginRequestDto loginRequestDto)
        {



            if (ModelState.IsValid)
            {

                string UserType = await _authService.ValidateUser(loginRequestDto.Email, loginRequestDto.Password);

                if (!string.IsNullOrEmpty(UserType))
                {
                    if (UserType=="Member") 
                    {
                        return RedirectToAction("Index", "MemberHome");
                    }

                    else if (UserType == "Trainer")
                    {
                        return RedirectToAction("Index","TrainerHome");
                    }

                    else
                    {
                        return RedirectToAction("Index","GymHome");
                    }
                }

                else if (await _authService.ValidateAdmin(loginRequestDto.Email, loginRequestDto.Password))
                {
                    return RedirectToAction("Index", "AdminHome");
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