using interfaces.IRoom;
using shortid;
using shortid.Configuration;

namespace services.RoomService
{
    public class RoomService: IRoom
    {
        public RoomService(){}

        public string generateRoomUuid()
        {
            string uuid = ShortId.Generate(new GenerationOptions() {Length = 8});

            return uuid;
        }
    }
}
