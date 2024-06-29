using Microsoft.AspNetCore.SignalR;

namespace KevDevTools.Hubs
{
    public class ViewCounterHub : Hub
    {
        public static int TotalViews { get; set; } = 0;
        public static int CurrentViews { get; set; } = 0;

        public override Task OnConnectedAsync()
        {
            CurrentViews++;
            Clients.All.SendAsync("updateCurrentViews", CurrentViews).GetAwaiter().GetResult();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            CurrentViews--;
            Clients.All.SendAsync("updateCurrentViews", CurrentViews).GetAwaiter().GetResult();
            return base.OnDisconnectedAsync(exception);
        }

        public async Task NewWindowsLoaded()
        {
            TotalViews++;
            await Clients.All.SendAsync("updateTotalViews", TotalViews);
        }
    }
}
