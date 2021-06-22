using System.Threading.Tasks;
using SignalRChat.IHubsMessage;

namespace SignalRChat.IHubs
{
    public interface IChatHubs {
        Task Spam(ChatMessage message);

        Task ConnectEvent();
    }
}