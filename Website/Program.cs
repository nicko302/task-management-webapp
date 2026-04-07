using CRUD_Application.Data;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ApplicationDbContext>();

// MySQL DB connection
string _GetConnStringName = builder.Configuration.GetConnectionString("DefaultConnectionMySQL");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(_GetConnStringName, ServerVersion.AutoDetect(_GetConnStringName)));

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
