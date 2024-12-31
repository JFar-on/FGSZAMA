using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FGSZAMA.Data;
using FGSZAMA.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FGSZAMA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Index
        public async Task<IActionResult> Index()
        {
            var orders = await _context.ZamowienieModel.ToListAsync();
            var users = await _userManager.Users.ToListAsync();

            var orderViewModels = orders.Select(order => new OrderViewModel
            {
                Id = order.Id,
                UserEmail = users.FirstOrDefault(u => u.Id == order.UserId)?.Email,
                Zestaw = order.Zestaw,
                Kalorycznosc = order.Kalorycznosc,
                CenaZaDzien = order.CenaZaDzien,
                DataOd = order.DataOd,
                DataDo = order.DataDo,
                SumaCeny = order.SumaCeny
            }).ToList();

            return View(orderViewModels);
        }

        // GET: Admin/Details/5
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

            var user = await _userManager.FindByIdAsync(zamowienieModel.UserId);
            var viewModel = new OrderViewModel
            {
                Id = zamowienieModel.Id,
                UserEmail = user?.Email,
                Zestaw = zamowienieModel.Zestaw,
                Kalorycznosc = zamowienieModel.Kalorycznosc,
                CenaZaDzien = zamowienieModel.CenaZaDzien,
                DataOd = zamowienieModel.DataOd,
                DataDo = zamowienieModel.DataDo,
                SumaCeny = zamowienieModel.SumaCeny
            };

            return View(viewModel);
        }

        // GET: Admin/Edit/5
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

            var user = await _userManager.FindByIdAsync(zamowienieModel.UserId);
            var viewModel = new OrderViewModel
            {
                Id = zamowienieModel.Id,
                UserEmail = user?.Email,
                Zestaw = zamowienieModel.Zestaw,
                Kalorycznosc = zamowienieModel.Kalorycznosc,
                CenaZaDzien = zamowienieModel.CenaZaDzien,
                DataOd = zamowienieModel.DataOd,
                DataDo = zamowienieModel.DataDo,
                SumaCeny = zamowienieModel.SumaCeny
            };

            return View(viewModel);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Zestaw,Kalorycznosc,CenaZaDzien,DataOd,DataDo,SumaCeny,UserEmail")] OrderViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var zamowienieModel = await _context.ZamowienieModel.FindAsync(id);
                    if (zamowienieModel == null)
                    {
                        return NotFound();
                    }

                    zamowienieModel.Zestaw = viewModel.Zestaw;
                    zamowienieModel.Kalorycznosc = viewModel.Kalorycznosc;
                    zamowienieModel.CenaZaDzien = viewModel.CenaZaDzien;
                    zamowienieModel.DataOd = viewModel.DataOd;
                    zamowienieModel.DataDo = viewModel.DataDo;
                    zamowienieModel.SumaCeny = viewModel.SumaCeny;

                    _context.Update(zamowienieModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZamowienieModelExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Admin/Delete/5
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

            var user = await _userManager.FindByIdAsync(zamowienieModel.UserId);
            var viewModel = new OrderViewModel
            {
                Id = zamowienieModel.Id,
                UserEmail = user?.Email,
                Zestaw = zamowienieModel.Zestaw,
                Kalorycznosc = zamowienieModel.Kalorycznosc,
                CenaZaDzien = zamowienieModel.CenaZaDzien,
                DataOd = zamowienieModel.DataOd,
                DataDo = zamowienieModel.DataDo,
                SumaCeny = zamowienieModel.SumaCeny
            };

            return View(viewModel);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zamowienieModel = await _context.ZamowienieModel.FindAsync(id);
            if (zamowienieModel != null)
            {
                _context.ZamowienieModel.Remove(zamowienieModel);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ZamowienieModelExists(int id)
        {
            return _context.ZamowienieModel.Any(e => e.Id == id);
        }
    }

    public class OrderViewModel
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string Zestaw { get; set; }
        public int Kalorycznosc { get; set; }
        public decimal CenaZaDzien { get; set; }
        public DateTime DataOd { get; set; }
        public DateTime DataDo { get; set; }
        public decimal SumaCeny { get; set; }
    }
}
