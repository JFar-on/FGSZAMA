using Microsoft.AspNetCore.Mvc;

namespace FGSZAMA.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Panel()
        {
            return View();
        }
    }
}
