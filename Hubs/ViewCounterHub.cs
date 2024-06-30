using KevDevTools.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace KevDevTools.Hubs
{
    public class ViewCounterHub : Hub
    {
        private static ConcurrentDictionary<string, string> SessionConnectionMap = new ConcurrentDictionary<string, string>();
        public static int TotalViews { get; set; } = 0;
        public static int CurrentViews { get; set; } = 0;

        public override async Task OnConnectedAsync()
        {
            CurrentViews++;
            Clients.All.SendAsync("updateCurrentViews", CurrentViews).GetAwaiter().GetResult();
            await base.OnConnectedAsync();
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
