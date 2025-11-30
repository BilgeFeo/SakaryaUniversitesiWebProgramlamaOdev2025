using Microsoft.AspNetCore.Mvc;

namespace WebProgramlamaOdev.Controllers.AdminPageControllers

{

    public class AdminHomeController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }


        public IActionResult ShowGyms()
        {
            return View();     
        }
    

        public IActionResult ShowServices() 
        {
            return View();
        }

        public IActionResult ShowTrainers()
        {
            return View();
        }

        public IActionResult ShowAppointments()
        {
            return View();
        }



        public IActionResult ShowMembers()
        {
            return View();
        }
    
    }
}
