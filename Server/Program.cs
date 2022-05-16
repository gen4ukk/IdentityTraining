using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Server;
using Server.Data;

var builder = WebApplication.CreateBuilder(args);


var assembly = typeof(Program).Assembly.GetName().Name;
var defaultConnString = builder.Configuration.GetConnectionString("IdentityConnection");

//start it once to setup seed data
//SeedData.EnsureSeedData(defaultConnString);

builder.Services.AddDbContext<AspNetIdentityDbContext>(options =>
    options.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly)));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AspNetIdentityDbContext>();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = context =>
        context.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
    })
    .AddOperationalStore(options =>
        {
            options.ConfigureDbContext = context =>
            context.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly(assembly));
        })
    .AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();

var app = builder.Build();
app.UseStaticFiles();
app.UseIdentityServer();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();
