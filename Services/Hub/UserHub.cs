using Microsoft.AspNetCore.SignalR;
using Services.Interface;
namespace Services.Hub
{
    public class UserHub : Hub<IUserHubClient>
    {
        public static int TotalViews { get; set; } = 0;
        public async Task NewWindowLoaded()
        {
            TotalViews++;
            // update total views ke semua client yang terkoneksi ke user hub
            await Clients.All.UpdateTotalViews(TotalViews);
        }
    }
}