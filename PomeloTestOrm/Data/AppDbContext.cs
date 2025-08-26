using Microsoft.EntityFrameworkCore;
using PomeloTestOrm.Models;

namespace PomeloTestOrm.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
    }
}
