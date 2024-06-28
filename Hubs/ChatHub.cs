using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChitChat_Server.Hubs {
    public class ChatHub : Hub {
        public override async Task OnConnectedAsync() {
            if (Context.User is {
                Identity: not null
            }) {
                await Broadcast("User [" + Context.User.Identity.Name + "] has joined the server.");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception) {
            if (Context.User is {
                Identity: not null
            }) {
                await Broadcast("User [" + Context.User.Identity.Name + "] has leaved the server.");
            }
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