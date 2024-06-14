using DACS_Web_Viec_Lam.Data;
using DACS_Web_Viec_Lam.Interface;
using DACS_Web_Viec_Lam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); 



builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
 .AddDefaultTokenProviders()
 .AddDefaultUI()
 .AddEntityFrameworkStores<ApplicationDbContext>();
//thong bao loi quyen truy cap
var configuration = builder.Configuration;
builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        IConfigurationSection googleAuthNSection = configuration.GetSection("Authentication:Google");


        googleOptions.ClientId = googleAuthNSection["ClientId"];
        googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
        // Cau hinh Url callback l?i t? Google 
        googleOptions.CallbackPath = "/dang-nhap-tu-google";
    }
    );
builder.Services.Configure<IdentityOptions>(options =>
{
    // Thi?t l?p Lockout.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.AllowedForNewUsers = true;
}); 

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.LogoutPath = $"/Identity/Account/AccessDenied";

});
builder.Services.AddScoped<IJobSeekerRepository, EFJobSeekerRepository>();
builder.Services.AddScoped<IJobSeeker1Repository, EFJobSeeker1Repository>();
builder.Services.AddScoped<ICompanyRepository, EFCompanyRepository>();
builder.Services.AddScoped<IJobRepository, EFJobRepository>();
builder.Services.AddScoped<ITitleRepository, EFTitleRepository>();
builder.Services.AddScoped<INotificationRepository, EFNotificationRepository>();

builder.Services.AddRazorPages();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); ;
app.UseAuthorization();
app.MapRazorPages();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(name: "Admin", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(name: "JobSeeker", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(name: "Company", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(name: "Employer", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    //endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

});
app.MapControllerRoute(

 name: "default",
 pattern: "{controller=Employer}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
//app.MapControllerRoute(
//name: "Employer",
//pattern: "{area:exists}/{controller=Employer}/{action=Index}/{id?}"
//);
app.Run();