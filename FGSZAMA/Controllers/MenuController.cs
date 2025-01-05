using FGSZAMA.Models;
using FGSZAMA.Services;
using Microsoft.AspNetCore.Mvc;

namespace FGSZAMA.Controllers
{
    public class MenuController : Controller
    {
        private readonly AktywnośćService _aktywnośćService;

        public MenuController(AktywnośćService aktywnośćService)
        {
            _aktywnośćService = aktywnośćService;
        }
        public async Task<IActionResult> Menu()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _aktywnośćService.AddAktywnośćAsync(new AktywnośćModel
                {
                    NazwaUżytkownika = User.Identity.Name,
                    DataAktywności = DateTime.Now,
                    TypAktywności = "Wyświetlenie strony",
                    Opis = "Użytkownik wyświetlił stronę menu."
                });
            }


            return View();
        }
    }
}
