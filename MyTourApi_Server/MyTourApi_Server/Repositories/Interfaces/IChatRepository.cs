using MyTourApi_Server.Models;
using MyTourApi_Server.DTOs.Request;

namespace MyTourApi_Server.Repositories.Interfaces
{
    public interface IChatRepository
    {
        ChatRoom? GetOrCreateShareRoom();

        List<ChatMessage> GetMessages(int chatRoomId);

        bool AddMessage(ChatMessageRequest request);

        int ShareTouristSpot(int chatRoomId, ShareTouristSpotRequest request);

        bool ExistsMember(int memberId);

        bool ExistsTouristSpot(int spotId);
    }
}