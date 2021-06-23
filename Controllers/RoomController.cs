using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using interfaces.IRoom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using services.RoomService;
using shortid;
using shortid.Configuration;
using SignalRChat.Hubs;
using SignalRChat.IHubsMessage;

namespace EasyTransfer.Controllers
{
    [ApiController]
    [Route("/api/rooms")]
    public class RoomController : ControllerBase
    {
        private readonly ILogger<RoomController> _logger;

        private readonly IHubContext<ChatHub> _hubContext;

        private readonly IRoom _IRoom;

        public RoomController(ILogger<RoomController> logger, IHubContext<ChatHub> hub, IRoom iRoom)
        {
            _logger = logger;
            _hubContext = hub;
            _IRoom = iRoom;
        }

        [HttpGet]
        public IActionResult CreateRoom([FromQuery]QueryParameters parameters)  // 
        {
            string roomId = _IRoom.generateRoomUuid();
        
            dynamic Response = new ExpandoObject();
            Response.roomId = roomId;
            Response.connectionId = parameters.ConnectionId;
            
            _hubContext.Groups.AddToGroupAsync(parameters.ConnectionId, roomId);
            

            // ChatMessage msg = new ChatMessage();
            // msg.User = "sylvain";
            // msg.Message = "BONJOUR";
            // _hubContext.Clients.Group(roomId).Spam(msg);

            return Ok(Response);
        }
    }
}
