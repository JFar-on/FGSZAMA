using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FGSZAMA.Data;
using FGSZAMA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FGSZAMA
{
    [Authorize]
    public class ZamowienieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;


        public ZamowienieController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Zamowienie
        public async Task<IActionResult> Index()
        {

            var user = await _userManager.GetUserAsync(User);
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (isAdmin)
            {
            return View(await _context.ZamowienieModel.ToListAsync());
                
            }
            else
            {
                var userId = _userManager.GetUserId(User);
                var userOrders = await _context.ZamowienieModel.Where(z => z.UserId == userId).ToListAsync();
                return View(userOrders);
            }
        }

        // GET: Zamowienie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienieModel = await _context.ZamowienieModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zamowienieModel == null)
            {
                return NotFound();
            }

            return View(zamowienieModel);
        }

        // GET: Zamowienie/Create
        public IActionResult Create()
        {
            var model = new ZamowienieViewModel();
            return View(model);
        }

        // POST: Zamowienie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,WybranyZestaw,WybranaKalorycznosc,DataOd,DataDo")] ZamowienieViewModel model)
        {
            if (model.DataOd < DateTime.Today || model.DataDo < DateTime.Today)
            {
                ModelState.AddModelError(string.Empty, "Data nie może być wcześniejsza niż dzisiejsza.");
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = _userManager.GetUserId(User);

                // Pobranie ceny na podstawie wybranej kaloryczności
                var cenaZaDzien = model.OpcjeKalorycznosci
                    .FirstOrDefault(k => k.Kalorycznosc == model.WybranaKalorycznosc)?.Cena ?? 0;

                // Obliczenie liczby dni
                var liczbaDni = (model.DataDo - model.DataOd).Days + 1;

                // Obliczenie sumy ceny
                model.PodsumowanaCena = liczbaDni * cenaZaDzien;

                // Tworzenie nowego zamówienia
                var zamowienie = new ZamowienieModel
                {
                    Zestaw = model.WybranyZestaw,
                    Kalorycznosc = model.WybranaKalorycznosc,
                    CenaZaDzien = cenaZaDzien,
                    DataOd = model.DataOd,
                    DataDo = model.DataDo,
                    SumaCeny = model.PodsumowanaCena,
                    UserId = userId // Przypisz UserId
                };

                // Zapis do bazy danych
                _context.ZamowienieModel.Add(zamowienie);
                _context.SaveChanges();

                ViewBag.Sukces = "Zamówienie zostało poprawnie stworzone!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Zamowienie/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienieModel = await _context.ZamowienieModel.FindAsync(id);
            if (zamowienieModel == null)
            {
                return NotFound();
            }
            return View(zamowienieModel);
        }

        // POST: Zamowienie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    

        // GET: Zamowienie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zamowienieModel = await _context.ZamowienieModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zamowienieModel == null)
            {
                return NotFound();
            }

            return View(zamowienieModel);
        }

        // POST: Zamowienie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zamowienieModel = await _context.ZamowienieModel.FindAsync(id);
            if (zamowienieModel != null)
            {
                _context.ZamowienieModel.Remove(zamowienieModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZamowienieModelExists(int id)
        {
            return _context.ZamowienieModel.Any(e => e.Id == id);
        }
    }
}
