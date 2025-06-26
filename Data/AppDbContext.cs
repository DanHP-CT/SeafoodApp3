using Microsoft.EntityFrameworkCore;
using SeafoodApp.Models.Entities;

namespace SeafoodApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Allocation> Allocations { get; set; }
        public DbSet<ProductionOrder> ProductionOrders { get; set; }
        public DbSet<ProductionOrderDetail> ProductionOrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Supplier>()
                .HasQueryFilter(s => !s.IsDeleted);

            modelBuilder.Entity<PurchaseOrder>()
                .HasQueryFilter(po => !po.IsDeleted);

            modelBuilder.Entity<PurchaseOrderDetail>()
                .HasQueryFilter(d => !d.IsDeleted);

            modelBuilder.Entity<Lot>()
                .HasQueryFilter(l => !l.IsDeleted);

            modelBuilder.Entity<Allocation>()
                .HasQueryFilter(a => !a.IsDeleted);

            modelBuilder.Entity<ProductionOrder>()
                .HasQueryFilter(po => !po.IsDeleted);

            modelBuilder.Entity<ProductionOrderDetail>()
                .HasQueryFilter(d => !d.IsDeleted);
        }
    }
}