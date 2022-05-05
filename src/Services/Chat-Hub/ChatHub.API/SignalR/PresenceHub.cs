using Application.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatHub.API.SignalR
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly PresenceTracker _tracker;

        public PresenceHub(PresenceTracker tracker)
        {
            _tracker = tracker;
        }
        /// <inheritdoc />
        public override async Task OnConnectedAsync()
        {
            var username = Context.User!.GetRequiredUsername();
            var isOnline = await _tracker.UserConnected(username, Context.ConnectionId);
            if (isOnline)
                await Clients.Others.SendAsync("UserIsOnline", username);
            //return base.OnConnectedAsync();
            var onlineUsers = await _tracker.GetOnlineUsers();
            //await Clients.All.SendAsync("GetOnlineUsers", onlineUsers);
            await Clients.Caller.SendAsync("GetOnlineUsers", onlineUsers);

        }

        /// <inheritdoc />
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var isOffline = await _tracker.UserDisconnected(Context.User!.GetRequiredUsername(), Context.ConnectionId);
            if (isOffline)
                await Clients.Others.SendAsync("UserIsOffline", Context.User!.GetRequiredUsername());

            //var onlineUsers = await _tracker.GetOnlineUsers();
            //await Clients.All.SendAsync("GetOnlineUsers", onlineUsers);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
