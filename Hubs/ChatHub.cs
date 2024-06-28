﻿using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs {
    public class ChatHub : Hub {
        public async Task SendMessage(string user, string message) {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task Broadcast(string user, string message) {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        
        public async Task Broadcast(string message) {
            await Clients.All.SendAsync("ReceiveMessage", "Server", message);
        }
    }
}