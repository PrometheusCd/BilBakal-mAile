using Microsoft.AspNetCore.Mvc;
using BilBakalimAile.Services;
using BilBakalimAile.Models;
using BilBakalimAile.Data; // Veritabanı için gerekli

namespace BilBakalimAile.Controllers
{
    public class QuizController : Controller
    {
        private readonly QuizService _quizService;
        private readonly QuizDbContext _context; // Veritabanı bağlantısı

        // Hem Servisi hem de Veritabanını çağırıyoruz
        public QuizController(QuizService quizService, QuizDbContext context)
        {
            _quizService = quizService;
            _context = context;
        }

        public IActionResult Index()
        {
            var questions = _quizService.GetAllQuestions();
            int currentIndex = HttpContext.Session.GetInt32("CurrentIndex") ?? 0;

            if (currentIndex >= questions.Count)
            {
                return RedirectToAction("Final");
            }

            // Joker Kontrolü
            ViewBag.Joker50Used = HttpContext.Session.GetString("Joker50") == "Used";
            ViewBag.JokerAudienceUsed = HttpContext.Session.GetString("JokerAudience") == "Used";

            var currentQuestion = questions[currentIndex];
            return View(currentQuestion);
        }

        [HttpPost]
        public IActionResult Answer(char selectedOption)
        {
            var questions = _quizService.GetAllQuestions();
            int currentIndex = HttpContext.Session.GetInt32("CurrentIndex") ?? 0;
            int currentScore = HttpContext.Session.GetInt32("Score") ?? 0;

            var correctOption = questions[currentIndex].CorrectAnswer;

            if (selectedOption == correctOption)
            {
                HttpContext.Session.SetInt32("Score", currentScore + 10);
            }

            HttpContext.Session.SetInt32("CurrentIndex", currentIndex + 1);
            return RedirectToAction("Index");
        }

        public IActionResult TimeUp()
        {
            int currentIndex = HttpContext.Session.GetInt32("CurrentIndex") ?? 0;
            HttpContext.Session.SetInt32("CurrentIndex", currentIndex + 1);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UseJoker(string jokerType)
        {
            HttpContext.Session.SetString("Joker" + jokerType, "Used");
            return Ok();
        }

        public IActionResult Final()
        {
            int score = HttpContext.Session.GetInt32("Score") ?? 0;
            ViewBag.TotalScore = score;

            // Oyun bitince oturumu sıfırlıyoruz ama skor kaydetmek için session'ı temizlemiyoruz
            // Sadece index'i sıfırlıyoruz ki "Tekrar Oyna" diyebilsin
            HttpContext.Session.SetInt32("CurrentIndex", 0);

            // Jokerleri sıfırla
            HttpContext.Session.Remove("Joker50");
            HttpContext.Session.Remove("JokerAudience");

            return View();
        }

        // --- YENİ EKLENEN: SKOR KAYDETME VE LİSTELEME ---

        [HttpPost]
        public IActionResult SaveScore(string playerName)
        {
            int score = HttpContext.Session.GetInt32("Score") ?? 0;

            var newScore = new Score
            {
                PlayerName = playerName,
                Points = score,
                Date = DateTime.Now
            };

            _context.Scores.Add(newScore);
            _context.SaveChanges();

            // Puanı da artık silebiliriz
            HttpContext.Session.SetInt32("Score", 0);

            return RedirectToAction("Leaderboard");
        }

        public IActionResult Leaderboard()
        {
            // En yüksek puana göre ilk 10 kişiyi getir
            var topScores = _context.Scores
                .OrderByDescending(s => s.Points)
                .ThenByDescending(s => s.Date)
                .Take(10)
                .ToList();

            return View(topScores);
        }
    }
}