using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChitChat_Server.Hubs {
    public class ChatHub : Hub {
        public override async Task OnConnectedAsync() {
            await base.OnConnectedAsync();
            HttpContext? httpContext = Context.GetHttpContext();
            var connectionId = Context.ConnectionId;
            var username = httpContext != null ? httpContext.Request.Query["username"].ToString() : connectionId;
            await UserJoined(connectionId, username);
        }

        public override async Task OnDisconnectedAsync(Exception? exception) {
            Context.GetHttpContext();
            var connectionId = Context.ConnectionId;
            await UserLeft(connectionId, "");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message) {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task Broadcast(string message) {
            await Clients.All.SendAsync("ReceiveMessage", "Server", message);
        }

        public async Task SendPrivateMessage(string connectionId, string user, string message) {
            await Clients.Client(connectionId).SendAsync("ReceiveMessage", user, message);
        }

        private async Task UserJoined(string connectionID, string user) {
            await Clients.All.SendAsync("UserJoined", connectionID, user);
        }

        private async Task UserLeft(string connectionID, string user) {
            await Clients.All.SendAsync("UserLeft", connectionID, user);
        }
    }
}