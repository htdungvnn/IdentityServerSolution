using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5001, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

// Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString).ConfigureWarnings(warnings =>
               warnings.Ignore(RelationalEventId.PendingModelChangesWarning)));

//builder.Services.AddDefaultIdentity<IdentityUser<Guid>>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
})
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(Program).Assembly.FullName));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(Program).Assembly.FullName));

        options.EnableTokenCleanup = true;
        options.TokenCleanupInterval = 30; // In seconds
    })
    .AddAspNetIdentity<IdentityUser<Guid>>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Auto-create and seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Apply migrations for Identity Database
    var applicationDbContext = services.GetRequiredService<ApplicationDbContext>();
    applicationDbContext.Database.Migrate();

    // Apply migrations for IdentityServer Configuration Store
    var configurationDbContext = services.GetRequiredService<ConfigurationDbContext>();
    configurationDbContext.Database.Migrate();

    // Apply migrations for IdentityServer Operational Store
    var persistedGrantDbContext = services.GetRequiredService<PersistedGrantDbContext>();
    persistedGrantDbContext.Database.Migrate();

    if (!configurationDbContext.Clients.Any())
    {
        foreach (var client in Config.Clients)
        {
            configurationDbContext.Clients.Add(client.ToEntity());
        }
        configurationDbContext.SaveChanges();
    }

    if (!configurationDbContext.IdentityResources.Any())
    {
        foreach (var resource in Config.IdentityResources)
        {
            configurationDbContext.IdentityResources.Add(resource.ToEntity());
        }
        configurationDbContext.SaveChanges();
    }

    if (!configurationDbContext.ApiScopes.Any())
    {
        foreach (var apiScope in Config.ApiScopes)
        {
            configurationDbContext.ApiScopes.Add(apiScope.ToEntity());
        }
        configurationDbContext.SaveChanges();
    }

    // Seed IdentityServer configuration data (clients, resources, API scopes)
    SeedIdentityServerData(configurationDbContext);
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();

// Seed IdentityServer data
void SeedIdentityServerData(ConfigurationDbContext configurationDbContext)
{
    if (!configurationDbContext.Clients.Any())
    {
        foreach (var client in Config.Clients)
        {
            configurationDbContext.Clients.Add(client.ToEntity());
        }
        configurationDbContext.SaveChanges();
    }

    if (!configurationDbContext.IdentityResources.Any())
    {
        foreach (var resource in Config.IdentityResources)
        {
            configurationDbContext.IdentityResources.Add(resource.ToEntity());
        }
        configurationDbContext.SaveChanges();
    }

    if (!configurationDbContext.ApiScopes.Any())
    {
        foreach (var apiScope in Config.ApiScopes)
        {
            configurationDbContext.ApiScopes.Add(apiScope.ToEntity());
        }
        configurationDbContext.SaveChanges();
    }
}