using Microsoft.AspNetCore.Mvc;
using BilBakalimAile.Data;
using BilBakalimAile.Models;

namespace BilBakalimAile.Controllers
{
    public class AdminController : Controller
    {
        private readonly QuizDbContext _context;

        public AdminController(QuizDbContext context)
        {
            _context = context;
        }

        // 1. GİRİŞ KAPISI (Şifre Ekranı)
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string password)
        {
            // ŞİFRE BURADA: "1234" (İstersen değiştirebilirsin)
            if (password == "Arda680168")
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Hatalı şifre, davetsiz misafir! 🕵️‍♂️";
            return View();
        }

        // 2. YÖNETİM PANELİ (Soruları Listele)
        public IActionResult Index()
        {
            // Güvenlik Kontrolü: Giriş yapmamışsa Login'e at
            if (HttpContext.Session.GetString("IsAdmin") != "true")
                return RedirectToAction("Login");

            // Soruları sondan başa doğru (en yeni en üstte) getir
            var questions = _context.Questions.OrderByDescending(q => q.Id).ToList();
            return View(questions);
        }

        // 3. YENİ SORU EKLEME SAYFASI (Formu Göster)
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true") return RedirectToAction("Login");
            return View();
        }

        // Form gönderilince burası çalışır ve kaydeder
        [HttpPost]
        public IActionResult Create(Question q)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true") return RedirectToAction("Login");

            if (ModelState.IsValid)
            {
                _context.Questions.Add(q);
                _context.SaveChanges(); // Veritabanına yaz
                return RedirectToAction("Index"); // Listeye dön
            }
            return View(q);
        }

        // 4. SORU SİLME
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true") return RedirectToAction("Login");

            var q = _context.Questions.Find(id);
            if (q != null)
            {
                _context.Questions.Remove(q);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // 5. ÇIKIŞ YAP
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("IsAdmin");
            return RedirectToAction("Index", "Quiz"); // Oyun ana sayfasına dön
        }
    }
}