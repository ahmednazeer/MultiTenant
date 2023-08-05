namespace MultiTenant.Models
{
    public class Product : ISharedTenant
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Rate { get; set; }
        public string TenantId { get; set; } = null!;
    }
}
