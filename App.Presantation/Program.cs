using Microsoft.AspNetCore.Identity;
using App.Common.DataAccess.Context.Concrete;
using App.Common.Entities.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppContextDB>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<tblUser,IdentityRole>(options =>
{
    // Identity seçeneklerini burada yapılandırabilirsiniz
})
    .AddEntityFrameworkStores<AppContextDB>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<tblUser>>();
builder.Services.AddScoped<SignInManager<tblUser>>();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache(); // Bellek tabanlı dağıtılmış önbellek ekleyin

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(20); // Oturumun zaman aşımı süresi
	options.Cookie.HttpOnly = true; // Sadece HTTP üzerinden erişilebilir çerezler
	options.Cookie.IsEssential = true; // Çerezler zorunlu
									   // Oturum seçeneklerini burada yapılandırabilirsiniz
});

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

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
