using Microsoft.EntityFrameworkCore;
using MultiTenant.Models;
using MultiTenant.Services;

namespace MultiTenant.Data
{
    public class AppDBContext:DbContext
    {
        public string TenantId { get; set; }
        private ITenantService _tenantService;

        public AppDBContext(DbContextOptions options, ITenantService tenantService) :base(options)
        {
            _tenantService = tenantService;
            TenantId = _tenantService.GetCurrentTenant()?.TenantId;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //to make a apply query filter to all select queries on prodcut entity
            modelBuilder.Entity<Product>().HasQueryFilter(t=>t.TenantId == TenantId);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString= _tenantService.GetTenantConnectionString();
            //chack db provider
            if(!string.IsNullOrEmpty (tenantConnectionString))
            {
                var dbProvider = _tenantService.GetTDBProvider();
                if (dbProvider.ToLower() == "mssql")
                    optionsBuilder.UseSqlServer(tenantConnectionString);
            }
            base.OnConfiguring(optionsBuilder);
        }

        public override Task<int> SaveChangesAsync( CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries<Product>().Where(e=>e.State== EntityState.Added))
            {
                item.Entity.TenantId = TenantId;
            }
            return base.SaveChangesAsync( cancellationToken);
        }
        public DbSet<Product> Products { get; set; }
    }

}
