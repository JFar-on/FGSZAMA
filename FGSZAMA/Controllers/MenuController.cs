using Microsoft.AspNetCore.Mvc;

namespace FGSZAMA.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Menu()
        {
            return View();
        }
    }
}
