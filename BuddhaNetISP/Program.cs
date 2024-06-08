using BuddhaNetISP.Helpers;
using BuddhaNetISP.Implementation;
using BuddhaNetISP.Interface;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
ConfigurationManager config = builder.Configuration;

builder.Services.Configure<ConnectionString>(config.GetSection("ConnectionStrings"));

builder.Services.AddTransient<IEquipmenrRepo, EquipmentRepo>();
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
