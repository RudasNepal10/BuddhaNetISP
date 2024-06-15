using BuddhaNetISP.Helpers;
using BuddhaNetISP.Implementation;
using BuddhaNetISP.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
 builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index"; 
        // Other options as needed
    });

// Add services to the container.
builder.Services.AddControllersWithViews();
ConfigurationManager config = builder.Configuration;

builder.Services.Configure<ConnectionString>(config.GetSection("ConnectionStrings"));

builder.Services.AddTransient<IEquipmenrRepo, EquipmentRepo>();
builder.Services.AddScoped<IHelpdeskoperatorRepo, HelpdeskoperatorRepo>();
builder.Services.AddScoped<IPersonalRepo, PersonalRepo>();
builder.Services.AddScoped<IProblemtypeRepo, ProblemtypeRepo>();
builder.Services.AddScoped<ISoftwareRepo, SoftwareRepo>();
builder.Services.AddScoped<IspecialistRepo, SpecialistRepo>();
builder.Services.AddScoped<IHelpdeskqueriesRepo, HelpdeskqueriesRepo>();
builder.Services.AddScoped<IuserRepo, UserRepo>();
//builder.Services.AddScoped<IcategoryRepo, CategoryRepo>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
