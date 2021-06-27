using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SignalRChat.IHubsMessage;
using System;
using System.Threading;
using wsDto.ResponseMessage;
using StackExchange.Redis;
using System.Net;

namespace SignalRChat.Hubs
{
    
    public class ChatHub : Hub
    {
        private readonly ILogger _logger;

        private readonly IConnectionMultiplexer _redis;

        public ChatHub(ILogger<ChatHub> logger, IConnectionMultiplexer redis)
        {
            _logger = logger;
            _redis = redis;
        }

        public async Task AddToGrp(string slugUuidRoomName) {
            this._logger.LogInformation($" New connection {Context.ConnectionId} joins roomId : {slugUuidRoomName}");

            await _redis.GetDatabase(0).StringSetAsync("ok", slugUuidRoomName);

            await Groups.AddToGroupAsync(Context.ConnectionId, slugUuidRoomName);
        }

        public async Task SendToGroup(string msg, string roomId) {
            this._logger.LogInformation("Send message to room Id : " + roomId);
            ResponseMessage responseMessage = new ResponseMessage(){Message=msg, SenderId=Context.ConnectionId};
            await Clients.Group(roomId).SendAsync("grpMessage",responseMessage);
        }

        public async Task SendFileToGroup(string b64File, string roomId) {
            this._logger.LogInformation("Upload file to room Id : " + roomId);
            await Clients.Group(roomId).SendAsync("grpFile", b64File);
        }

        public override Task OnConnectedAsync()
        {

            // _redis.GetDatabase(0).StringSet(Context.ConnectionId, );


            // // EndPoint[] endpoints = this._redis.GetEndPoints();
            // // foreach ( EndPoint endpoint in endpoints ) {
            // //     var server = _redis.GetServer(endpoint);
            // //     var keys = server.Keys(pattern: "*");
            // //     Console.WriteLine(keys);
            // // }

            // string value = this._redis.GetDatabase(0).StringGet("ok");
            // this._logger.LogInformation($"ok : {value}");

            // string x = "abcdefg";
            // _redis.GetDatabase(0).StringSet("mykey", x);
            // string y = _redis.GetDatabase(0).StringGet("mykey");
            // Console.WriteLine(y); // writes: "abcdefg"

            // since we used interface for abstract class Hub, call "base" to access method already implemented in Hub (abstract class)


            _redis.GetDatabase(0).KeyDelete(Context.ConnectionId);


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