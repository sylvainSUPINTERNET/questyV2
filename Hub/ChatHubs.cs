using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalRChat.IHubsMessage;
using System;

namespace SignalRChat.Hubs
{
    
    public class ChatHub : Hub
    {
        private readonly ILogger _logger;
        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }

        public async Task AddToGrp(string slugUuidRoomName) {
            Console.WriteLine("RECEIVED ON ADD GRP" + slugUuidRoomName);
            await Groups.AddToGroupAsync(Context.ConnectionId, "TOTO");
            await Clients.Group("TOTO").SendAsync("GrpMessage", "HELLO YOU ARE BVITCH");
            Console.WriteLine("MESSAGE SEND TO THE ROOM " + "TOTO");
        }

        public override Task OnConnectedAsync()
        {

            // since we used interface for abstract class Hub, call "base" to access method already implemented in Hub (abstract class)

            base.OnConnectedAsync();
            this._logger.LogInformation("Connection new client : " + Context.ConnectionId);
            // Clients.All.SendAsync("updateCount", Count);
            // Clients.All.SendAsync("connected", Context.ConnectionId);
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            base.OnDisconnectedAsync(exception);
            this._logger.LogInformation("Disconnected client : " + Context.ConnectionId);

            return Task.CompletedTask;
        }
    }
}