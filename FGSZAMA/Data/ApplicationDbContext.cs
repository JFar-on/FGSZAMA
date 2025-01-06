using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FGSZAMA.Models;

namespace FGSZAMA.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FGSZAMA.Models.ZamowienieModel> ZamowienieModel { get; set; } = default!;
        public DbSet<AktywnoscModel> Aktywności{ get; set; }
    }
}
