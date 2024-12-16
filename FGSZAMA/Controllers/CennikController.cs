using Microsoft.AspNetCore.Mvc;

namespace FGSZAMA.Controllers
{
    public class CennikController : Controller
    {
        public IActionResult Cennik()
        {
            return View();
        }
    }
}
