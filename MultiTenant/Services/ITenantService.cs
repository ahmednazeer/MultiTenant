namespace MultiTenant.Services
{
    public interface ITenantService
    {
        string? GetTenantConnectionString();
        string? GetTDBProvider();
        Tenant? GetCurrentTenant();

    }
}
