namespace MultiTenant.Configurations
{
    public class Tenant
    {
        public string? ConnectionString { get; set; }
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
