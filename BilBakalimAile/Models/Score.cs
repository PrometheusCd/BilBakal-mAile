namespace BilBakalimAile.Models
{
    public class Score
    {
        public int Id { get; set; }
        public string PlayerName { get; set; } = string.Empty; // Oyuncunun Adı
        public int Points { get; set; } // Aldığı Puan
        public DateTime Date { get; set; } = DateTime.Now; // Oynadığı Tarih
    }
}