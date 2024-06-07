using Microsoft.AspNetCore.SignalR;
using Services.Interface;
namespace Services { 
    public class NotificationHub : Hub<INotificationHubClient> 
    { 
        public async Task SendMessage(string message) 
        {
            // mengirim sebuah message ke semua client yang terkoneksi ke notification hub
            await Clients.All.SendMessage(message);
        }
    }
}