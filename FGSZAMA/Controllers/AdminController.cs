using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FGSZAMA.Data;
using FGSZAMA.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;


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

        public IActionResult Index()
        {
            return View();
        }

        // GET: Admin/ZamowieniePanel
        public async Task<IActionResult> ZamowieniePanel()
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
        public async Task<IActionResult> ZPDetails(int? id)
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

     
        // GET: Admin/Delete/5
        public async Task<IActionResult> ZPDelete(int? id)
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
        [HttpPost, ActionName("ZPDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zamowienieModel = await _context.ZamowienieModel.FindAsync(id);
            if (zamowienieModel != null)
            {
                _context.ZamowienieModel.Remove(zamowienieModel);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ZamowieniePanel));
        }

        private bool ZamowienieModelExists(int id)
        {
            return _context.ZamowienieModel.Any(e => e.Id == id);
        }


        //-------------------------------------------------------------------------------------


        // GET: Admin/Users
        public async Task<IActionResult> UzytkownikPanel()
        {
            var users = await _userManager.Users.ToListAsync();
            var userViewModels = new List<UżytkownikViewModel>();

            foreach (var user in users)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                userViewModels.Add(new UżytkownikViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = claims.FirstOrDefault(c => c.Type == "FirstName")?.Value,
                    LastName = claims.FirstOrDefault(c => c.Type == "LastName")?.Value,
                    Description = claims.FirstOrDefault(c => c.Type == "Description")?.Value
                });
            }
            return View(userViewModels);
        }

        // GET: Admin/EditUser/5
        public async Task<IActionResult> UPEdit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var model = new UżytkownikViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = claims.FirstOrDefault(c => c.Type == "FirstName")?.Value,
                LastName = claims.FirstOrDefault(c => c.Type == "LastName")?.Value,
                Description = claims.FirstOrDefault(c => c.Type == "Description")?.Value
            };

            return View(model);
        }

        // POST: Admin/EditUser/5
        [HttpPost]
        public async Task<IActionResult>UPEdit(UżytkownikViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.Email = model.Email;
                var claims = await _userManager.GetClaimsAsync(user);
                var firstNameClaim = claims.FirstOrDefault(c => c.Type == "FirstName");
                var lastNameClaim = claims.FirstOrDefault(c => c.Type == "LastName");
                var descriptionClaim = claims.FirstOrDefault(c => c.Type == "Description");

                if (firstNameClaim != null)
                {
                    await _userManager.RemoveClaimAsync(user, firstNameClaim);
                }
                if (lastNameClaim != null)
                {
                    await _userManager.RemoveClaimAsync(user, lastNameClaim);
                }
                if (descriptionClaim != null)
                {
                    await _userManager.RemoveClaimAsync(user, descriptionClaim);
                }

                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("FirstName", model.FirstName));
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("LastName", model.LastName));
                await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("Description", model.Description));

                await _userManager.UpdateAsync(user);
                return RedirectToAction(nameof(UzytkownikPanel));
            }

            return View(model);
        }

        // GET: Admin/UPDelete/5
        public async Task<IActionResult> UPDelete(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var claims = await _userManager.GetClaimsAsync(user);
            return View(new UżytkownikViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = claims.FirstOrDefault(c => c.Type == "FirstName")?.Value,
                LastName = claims.FirstOrDefault(c => c.Type == "LastName")?.Value,
                Description = claims.FirstOrDefault(c => c.Type == "Description")?.Value
            });
        }

        // POST: Admin/DeleteUser/5
        [HttpPost, ActionName("UPDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(UzytkownikPanel));
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
