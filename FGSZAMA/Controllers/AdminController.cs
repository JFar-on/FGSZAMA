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
