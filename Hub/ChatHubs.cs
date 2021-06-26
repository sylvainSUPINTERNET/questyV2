using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalRChat.IHubsMessage;
using System;
using System.Threading;
using wsDto.ResponseMessage;

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
            this._logger.LogDebug($" New connection {Context.ConnectionId} joins roomId : {slugUuidRoomName}");
            await Groups.AddToGroupAsync(Context.ConnectionId, slugUuidRoomName);
        }

        public async Task SendToGroup(string msg, string roomId) {
            this._logger.LogDebug("Send message to room Id : " + roomId);
            ResponseMessage responseMessage = new ResponseMessage(){Message=msg, SenderId=Context.ConnectionId};
            await Clients.Group(roomId).SendAsync("grpMessage",responseMessage);
        }

        public async Task SendFileToGroup(string b64File, string roomId) {
            this._logger.LogDebug("Upload file to room Id : " + roomId);
            await Clients.Group(roomId).SendAsync("grpFile", b64File);
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