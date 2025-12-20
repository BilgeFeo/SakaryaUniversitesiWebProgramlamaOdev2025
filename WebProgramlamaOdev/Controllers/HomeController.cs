using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WebProgramlamaOdev.DTOs;
using WebProgramlamaOdev.Models;
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

        public async Task<IActionResult> Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            }
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

                var user = await _authService.GetUserByEmailAsync(loginRequestDto.Email);
                string userType = await _authService.ValidateUser(loginRequestDto.Email, loginRequestDto.Password);



                if (!string.IsNullOrEmpty(userType)&&user != null)
                {
                    
                     var claims = new List<Claim>
                     {
                        

                       new Claim(ClaimTypes.Name, user.Email),
                       new Claim(ClaimTypes.Role, userType), 
                       new Claim("UserId", user.Id.ToString()) 
                     };
                        claims.Add(new Claim("Id", user.Id.ToString()));
                    
                    
                    



                        
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);




                    await HttpContext.SignInAsync(
                    IdentityConstants.ApplicationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                    // 5. Yönlendirmeler
                    return userType switch
                    {
                        "Member" => RedirectToAction("Index", "MemberHome"),
                        "Trainer" => RedirectToAction("Index", "TrainerHome"),
                        "Gym" => RedirectToAction("Index", "AdminHome"),
                        _ => RedirectToAction("Index", "Home")
                    };
                }
                else if (await _authService.ValidateAdmin(loginRequestDto.Email, loginRequestDto.Password))
                {

                    var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, "Admin"), // Member, Trainer veya Gym
                    new Claim("UserId", user.Id.ToString()) // Genel ID
                    };

                    // Eğer Trainer ise ekstra TrainerId ekleyebilirsin (UserId ile aynıysa gerek yok ama özel bir ID ise eklenebilir)

                    claims.Add(new Claim("Id", user.Id.ToString()));


                    // 3. Identity ve Principal Oluştur
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);




                    await HttpContext.SignInAsync(
                    IdentityConstants.ApplicationScheme,
                    new ClaimsPrincipal(claimsIdentity));



                    return RedirectToAction("Index", "AdminHome");
                }
            }
            return View("Index");
        }

                
                
         

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}