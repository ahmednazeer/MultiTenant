using Microsoft.Extensions.Options;

namespace MultiTenant.Services
{
    public class TenantService : ITenantService
    {
        private Tenant _currentTenant;
        private HttpContext? _httpContext;
        private TenantSettings _tenantSettings;
        public TenantService(IHttpContextAccessor httpContextAccessor,IOptions<TenantSettings> options)
        {
            _tenantSettings = options.Value;
            _httpContext = httpContextAccessor.HttpContext;
            //get current tenant
            if(_httpContext is not null && _tenantSettings is not null)
            {
                _httpContext.Request.Headers.TryGetValue("tenant", out var tenantId);
                if (string.IsNullOrEmpty(tenantId))
                    throw new Exception("Invalid Tenant!");
                _currentTenant = SetCurrentTenant(tenantId!);
                }


        }
        public Tenant? GetCurrentTenant()
        {
            return _currentTenant;
        }

        public string? GetTDBProvider()
        {
            return _tenantSettings.Defaults.DbProvider ;
        }

        public string? GetTenantConnectionString()
        {
            return _currentTenant is null ? _tenantSettings.Defaults.ConnectionString : _currentTenant.ConnectionString;
        }

        private Tenant? SetCurrentTenant(string  tenantId)
        {
            
            Tenant currentTenant = _tenantSettings.Tenants.FirstOrDefault(t => t.Id == tenantId);
            if (currentTenant is not null && currentTenant.ConnectionString is null)
                    currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
            return currentTenant;
        }

    }
}
