using KevDevTools.Controllers;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace KevDevTools.Hubs
{
    public class ViewCounterHub : Hub
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static ConcurrentDictionary<string, string> SessionConnectionMap = new ConcurrentDictionary<string, string>();
        public static int TotalViews { get; set; } = 0;
        public static int CurrentViews { get; set; } = 0;

        public ViewCounterHub(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task OnConnectedAsync()
        {
            CurrentViews++;
            Clients.All.SendAsync("updateCurrentViews", CurrentViews).GetAwaiter().GetResult();

            var context = _httpContextAccessor.HttpContext;
            if(context != null)
            {
                var sessionId = context.Session.Id;
                SessionConnectionMap[sessionId] = Context.ConnectionId;
            }
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            CurrentViews--;
            Clients.All.SendAsync("updateCurrentViews", CurrentViews).GetAwaiter().GetResult();
            var context = _httpContextAccessor.HttpContext;
            if(context != null)
            {
                context.Session.Remove("ConnectionId");
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task NewWindowsLoaded()
        {
            TotalViews++;
            await Clients.All.SendAsync("updateTotalViews", TotalViews);
        }

        public static string GetConnectionIdForSession(string sessionId)
        {
            if (SessionConnectionMap.TryGetValue(sessionId, out string connectionId))
            {
                return connectionId;
            }
            return null;
        }
    }
}
