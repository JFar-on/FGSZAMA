using FGSZAMA.Models;
using FGSZAMA.Services;
using Microsoft.AspNetCore.Mvc;

namespace FGSZAMA.Controllers
{
    public class CennikController : Controller
    {
        private readonly AktywnoscService _aktywnośćService;

        public CennikController(AktywnoscService aktywnośćService)
        {
            _aktywnośćService = aktywnośćService;
        }
        public async Task<IActionResult> Cennik()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _aktywnośćService.AddAktywnośćAsync(new AktywnoscModel
                {
                    NazwaUżytkownika = User.Identity.Name,
                    DataAktywności = DateTime.Now,
                    TypAktywności = "Wyświetlenie strony",
                    Opis = "Użytkownik wyświetlił stronę cennika."
                });
            }


            return View();
        }
    }
}
