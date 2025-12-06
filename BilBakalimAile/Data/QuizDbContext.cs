using Microsoft.EntityFrameworkCore;
using BilBakalimAile.Models;

namespace BilBakalimAile.Data
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options)
        {
        }

        // Veritabanındaki "Sorular" tablosu
        public DbSet<Question> Questions { get; set; }
    }
}