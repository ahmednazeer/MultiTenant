namespace MultiTenant.Configurations;

public class TenantSettings
{
    public List<Tenant>? Tenants { get; set; } = null!;
    public Configuration? Defaults { get; set; }=null!;
}

