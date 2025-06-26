using EnglishDictionaryWeb.Data;                // Kendi DbContext ve modellerimizi kullanmak için
using EnglishDictionaryWeb.Models;              // AppUser sınıfını kullanmak için
using Microsoft.AspNetCore.Identity;            // Identity sistemini aktif etmek için
using Microsoft.EntityFrameworkCore;            // Veritabanı bağlantısı için

namespace EnglishDictionaryWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Uygulama builder başlatılıyor
            var builder = WebApplication.CreateBuilder(args);

            // *** Database Bağlantısı ***
            // appsettings.json içindeki "DefaultConnection" bilgisini kullanarak SQL bağlantısı sağlıyoruz
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // *** Identity Kullanıcı Sistemi ***
            // AppUser modelimizi kullanarak Identity sistemini ekliyoruz
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // *** Login/Logout yönlendirmeleri ***
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";      // Kullanıcı giriş yapmamışsa buraya yönlenir
                options.LogoutPath = "/Account/Logout";    // Çıkış işlemi sonrası buraya yönlenir
            });

            // MVC Controller ve View sistemini aktif ediyoruz
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // *** Hata Yönetimi ***
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");  // Hata sayfası yönlendirmesi
                app.UseHsts();                          // Güvenlik için HTTP Strict Transport Security
            }

            // HTTPS yönlendirme (HTTP istekleri otomatik HTTPS'e yönlenir)
            app.UseHttpsRedirection();

            // wwwroot klasöründen statik dosyalar (CSS, JS, görseller) servis edilir
            app.UseStaticFiles();

            // *** Routing başlatılır ***
            app.UseRouting();

            // *** Kimlik Doğrulama ve Yetkilendirme ***
            app.UseAuthentication();  // Kullanıcının login olup olmadığını kontrol eder
            app.UseAuthorization();   // Sayfalara erişim yetkisini kontrol eder

            // *** Varsayılan Route ayarı ***
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
