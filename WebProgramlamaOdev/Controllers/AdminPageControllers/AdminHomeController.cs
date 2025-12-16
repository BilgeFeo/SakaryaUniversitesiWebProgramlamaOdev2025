using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebProgramlamaOdev.Data;
using WebProgramlamaOdev.DTOs;
using WebProgramlamaOdev.Models;
using WebProgramlamaOdev.Repositories.Interfaces;
using WebProgramlamaOdev.Services.AdminPageServices;

namespace WebProgramlamaOdev.Controllers.AdminPageControllers

{

    public class AdminHomeController : Controller
    {
        
        private readonly ICreateGymService _createGymService;
        public AdminHomeController(ICreateGymService createGymService)
        {

            _createGymService = createGymService;


        }



        public IActionResult Index()
        {
            return View();
        }


        public IActionResult ShowGyms()
        {
            return View();     
        }
    

        public IActionResult CreateGym()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGymRequestDto model)
        {
            if (ModelState.IsValid)
            {
                

                if (!await _createGymService.CreateGymAndSaveOnDatabase(model))
                {
                    
                    return RedirectToAction(nameof(Index));
                }

                
            }

            return View(model);
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
