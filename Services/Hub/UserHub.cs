using Microsoft.AspNetCore.SignalR;
using Services.Interface;
namespace Services.Hub
{
    public class UserHub : Hub<IUserHubClient>
    {
        public static int TotalViews { get; set; } = 0;
        public static int TotalActiveUsers { get; set; } = 0;

        public override  Task OnConnectedAsync()
        {
            TotalActiveUsers++;
            Clients.All.UpdateTotalActiveUsers(TotalActiveUsers).GetAwaiter().GetResult() ;
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            TotalActiveUsers--;
            Clients.All.UpdateTotalActiveUsers(TotalActiveUsers).GetAwaiter().GetResult() ;
            return base.OnConnectedAsync();
        }
        public async Task NewWindowLoaded()
        {
            TotalViews++;
            // update total views ke semua client yang terkoneksi ke user hub
            await Clients.All.UpdateTotalViews(TotalViews);
        }
    }
}