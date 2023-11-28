using Microsoft.AspNetCore.Identity;
using App.Common.DataAccess.Context.Concrete;
using App.Common.Entities.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppContextDB>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<tblUser,IdentityRole>(options =>
{
    // Identity seçeneklerini burada yapýlandýrabilirsiniz
})
    .AddEntityFrameworkStores<AppContextDB>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<tblUser>>();
builder.Services.AddScoped<SignInManager<tblUser>>();

// Add services to the container.
builder.Services.AddControllersWithViews();



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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
