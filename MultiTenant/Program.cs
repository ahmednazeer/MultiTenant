

using Microsoft.EntityFrameworkCore;
using MultiTenant.Data;
using MultiTenant.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<TenantSettings>(builder.Configuration.GetSection(nameof(TenantSettings)));


//to bind confiuration to that object 
TenantSettings options = new();
//app.Configuration.Bind(options);
var connectionstring=builder.Configuration.GetSection("TenantSettings:Defaults:ConnectionString").Value;
var defaultDbProvider = builder.Configuration.GetSection("TenantSettings:Defaults:DbProvider").Value;
//TenantSettingsvar defaultDbProvider = options.Defaults.DbProvider;
if (!string.IsNullOrEmpty(defaultDbProvider))
    builder.Services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer());



//register servoces dependencies
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<IProductService, ProductService>();


var app = builder.Build();

app.Configuration.GetSection(nameof(TenantSettings)).Bind(options);

foreach (var tenant in options.Tenants)
{
    var connectionString = tenant.ConnectionString ?? options.Defaults.ConnectionString;
    var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetService<AppDBContext>();
    context.Database.SetConnectionString(connectionString);
    if (context.Database.GetMigrations().Any())
        context.Database.Migrate();

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
