using Microsoft.AspNetCore.Mvc;

namespace VidFluentAI.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
