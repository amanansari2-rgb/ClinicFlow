using ClinicFlow_Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow_Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Identity> identity { get; set; }
    }
}
