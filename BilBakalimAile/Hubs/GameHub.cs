using Microsoft.AspNetCore.SignalR;

namespace BilBakalimAile.Hubs
{
    public class GameHub : Hub
    {
        // Bu fonksiyonu tetiklediğimizde, bağlı olan HERKESE mesaj gider.
        public async Task SendNewQuestion(int questionId)
        {
            // "ReceiveQuestion" dinleyicisine sahip herkese ID'yi gönder
            await Clients.All.SendAsync("ReceiveQuestion", questionId);
        }
    }
}