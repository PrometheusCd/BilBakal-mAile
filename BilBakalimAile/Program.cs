using BilBakalimAile.Services;
using Microsoft.EntityFrameworkCore; // YENÝ
using BilBakalimAile.Data;         // YENÝ

var builder = WebApplication.CreateBuilder(args);

// Servisleri ekle
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// --- VERÝTABANI BAÐLANTISI ---
builder.Services.AddDbContext<QuizDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
// -----------------------------

// --- ÖNEMLÝ DEÐÝÞÝKLÝK ---
// Veritabaný kullandýðýmýz için "AddSingleton" yerine "AddScoped" yapýyoruz!
builder.Services.AddScoped<QuizService>();
// --------------------------

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Quiz}/{action=Index}/{id?}");

app.Run();