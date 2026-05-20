using MyTourApi_Server.DTOs.Request;
using MyTourApi_Server.DTOs.Response;

namespace MyTourApi_Server.Services.Interfaces
{
    public interface IChatService
    {
        ChatRoomResponse? GetOrCreateShareRoom();

        List<ChatMessageResponse> GetMessages(int chatRoomId);

        bool AddMessage(ChatMessageRequest request, out string message);

        ShareTouristSpotResponse? ShareTouristSpot(ShareTouristSpotRequest request, out string message);
    }
}