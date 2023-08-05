namespace MultiTenant.Configurations
{
    public class Tenant
    {
        public string? ConnectionString { get; set; }
        public string TenantId { get; set; } = null!;
        public string TenantName { get; set; } = null!;
    }
}
