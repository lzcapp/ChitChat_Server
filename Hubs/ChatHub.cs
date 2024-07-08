using Microsoft.AspNetCore.SignalR;

namespace ChitChat_Server.Hubs {
    public class ChatHub : Hub {
        public override async Task OnConnectedAsync() {
            await base.OnConnectedAsync();
            HttpContext? httpContext = Context.GetHttpContext();
            var connectionId = Context.ConnectionId;
            var username = httpContext != null ? httpContext.Request.Query["username"].ToString() : connectionId;
            //_users.Add(new KeyValuePair<string, string>(connectionId, username));
            await Clients.All.SendAsync("UserJoined", connectionId, username);
        }

        public override async Task OnDisconnectedAsync(Exception? exception) {
            var connectionId = Context.ConnectionId;
            var username = string.Empty;
            //foreach (KeyValuePair<string, string> user in _users) {
            //    if (user.Key != connectionId) {
            //        continue;
            //    }
            //    username = user.Value;
            //    break;
            //}
            if (username != string.Empty) {
                await Clients.All.SendAsync("UserLeft", connectionId, username);
            } else {
                await Clients.All.SendAsync("UserLeft", connectionId, string.Empty);
            }
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
    }
}