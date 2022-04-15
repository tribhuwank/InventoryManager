using InventoryManager.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.API.Data
{
    public class InventoryDb : DbContext
    {
        public InventoryDb(DbContextOptions<InventoryDb> options) : base(options) { }
        public DbSet<Product> Products => Set<Product>();
    }
}
