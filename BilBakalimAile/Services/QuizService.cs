using BilBakalimAile.Data;
using BilBakalimAile.Models;
using Microsoft.EntityFrameworkCore; // Veritabanı işlemleri için

namespace BilBakalimAile.Services
{
    public class QuizService
    {
        private readonly QuizDbContext _context;

        // Artık listeyi değil, veritabanı yöneticisini (context) alıyoruz
        public QuizService(QuizDbContext context)
        {
            _context = context;
        }

        public List<Question> GetAllQuestions()
        {
            // Eğer veritabanında hiç soru yoksa, otomatik olarak ilk soruları ekle
            if (!_context.Questions.Any())
            {
                SeedQuestions();
            }

            // Veritabanındaki tüm soruları listeye çevirip getir
            return _context.Questions.ToList();
        }

        // Veritabanı boşsa çalışacak ve ilk soruları ekleyecek metod
        private void SeedQuestions()
        {
            var defaultQuestions = new List<Question>
            {
                new Question { Text = "Türkiye'nin başkenti neresidir?", OptionA = "İstanbul", OptionB = "Ankara", OptionC = "İzmir", OptionD = "Bursa", CorrectAnswer = 'B' },
                new Question { Text = "Web sayfalarını renklendirmek için hangisi kullanılır?", OptionA = "HTML", OptionB = "CSS", OptionC = "SQL", OptionD = "C#", CorrectAnswer = 'B' },
                new Question { Text = "Hangisi bir programlama dili değildir?", OptionA = "Python", OptionB = "Java", OptionC = "Excel", OptionD = "C++", CorrectAnswer = 'C' },
                new Question { Text = "Dünya'nın uydusu hangisidir?", OptionA = "Güneş", OptionB = "Mars", OptionC = "Ay", OptionD = "Venüs", CorrectAnswer = 'C' },
                new Question { Text = "Futbol maçı kaç kişiyle oynanır (bir takım)?", OptionA = "10", OptionB = "11", OptionC = "12", OptionD = "7", CorrectAnswer = 'B' },
                // Buraya istediğin kadar yeni soru ekleyebilirsin!
                new Question { Text = "Hangi hayvan kış uykusuna yatar?", OptionA = "Ayı", OptionB = "Kedi", OptionC = "Köpek", OptionD = "At", CorrectAnswer = 'A' }
            };

            _context.Questions.AddRange(defaultQuestions);
            _context.SaveChanges(); // Değişiklikleri veritabanına kaydet
        }
    }
}