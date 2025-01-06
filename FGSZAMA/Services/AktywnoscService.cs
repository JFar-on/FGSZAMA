using FGSZAMA.Data;
using FGSZAMA.Models;
using Microsoft.EntityFrameworkCore;


namespace FGSZAMA.Services
{
    public class AktywnoscService
    {
        private readonly ApplicationDbContext _context;

        public AktywnoscService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AktywnoscModel>> GetAktywnościAsync()
        {
            return await _context.Aktywności.ToListAsync();
        }

        public async Task AddAktywnośćAsync(AktywnoscModel aktywność)
        {
            _context.Aktywności.Add(aktywność);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAktywnośćAsync(int id)
        {
            var aktywność = await _context.Aktywności.FindAsync(id);
            if (aktywność != null)
            {
                _context.Aktywności.Remove(aktywność);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkAsReadAsync(int id)
        {
            var aktywność = await _context.Aktywności.FindAsync(id);
            if (aktywność != null)
            {
                aktywność.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
