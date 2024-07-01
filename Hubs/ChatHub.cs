using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChitChat_Server.Hubs {
    public class ChatHub : Hub {
        public override async Task OnConnectedAsync() {
            await base.OnConnectedAsync();
            var username = "";
            if (Context?.GetHttpContext() != null) {
                username = Context.GetHttpContext().Request.Query["username"];
            } else {
                username = Context.ConnectionId;
            }
            await Broadcast($"[{username}] joined the server.");
        }

        public override async Task OnDisconnectedAsync(Exception? exception) {
            await Broadcast($"[{Context.ConnectionId}] leave the server.");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string user, string message) {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task Broadcast(string message) {
            await Clients.All.SendAsync("ReceiveMessage", "Server", message);
        }
    }
}