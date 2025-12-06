using Microsoft.AspNetCore.Mvc;
using BilBakalimAile.Services;
using BilBakalimAile.Models;

namespace BilBakalimAile.Controllers
{
    public class QuizController : Controller
    {
        private readonly QuizService _quizService;

        public QuizController(QuizService quizService)
        {
            _quizService = quizService;
        }

        public IActionResult Index()
        {
            var questions = _quizService.GetAllQuestions();
            int currentIndex = HttpContext.Session.GetInt32("CurrentIndex") ?? 0;

            if (currentIndex >= questions.Count)
            {
                return RedirectToAction("Final");
            }

            // --- JOKER KONTROLÜ (YENİ) ---
            // Session'a bakıyoruz: Daha önce kullanıldı mı?
            ViewBag.Joker50Used = HttpContext.Session.GetString("Joker50") == "Used";
            ViewBag.JokerAudienceUsed = HttpContext.Session.GetString("JokerAudience") == "Used";
            // -----------------------------

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

        // --- YENİ EKLENEN JOKER KAYIT METODU ---
        [HttpPost]
        public IActionResult UseJoker(string jokerType)
        {
            // JavaScript buraya istek atacak: "50" veya "Audience"
            HttpContext.Session.SetString("Joker" + jokerType, "Used");
            return Ok();
        }
        // ---------------------------------------

        public IActionResult Final()
        {
            int score = HttpContext.Session.GetInt32("Score") ?? 0;
            ViewBag.TotalScore = score;

            // Oyun bitince her şeyi sıfırla
            HttpContext.Session.SetInt32("CurrentIndex", 0);
            HttpContext.Session.SetInt32("Score", 0);

            // Jokerleri de sıfırla ki yeni oyunda tekrar kullanabilsinler
            HttpContext.Session.Remove("Joker50");
            HttpContext.Session.Remove("JokerAudience");

            return View();
        }
    }
}