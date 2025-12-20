using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebProgramlamaOdev.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberTrainerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}