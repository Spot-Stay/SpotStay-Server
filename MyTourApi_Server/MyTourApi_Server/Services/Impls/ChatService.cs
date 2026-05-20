using MyTourApi_Server.DTOs.Request;
using MyTourApi_Server.DTOs.Response;
using MyTourApi_Server.Models;
using MyTourApi_Server.Repositories.Interfaces;
using MyTourApi_Server.Services.Interfaces;

namespace MyTourApi_Server.Services.Impls
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            this.chatRepository = chatRepository;
        }

        public ChatRoomResponse? GetOrCreateShareRoom()
        {
            ChatRoom? room = chatRepository.GetOrCreateShareRoom();

            if (room == null)
                return null;

            return new ChatRoomResponse
            {
                ChatRoomId = room.ChatRoomId,
                RoomName = room.RoomName,
                CreatedAt = room.CreatedAt
            };
        }

        public List<ChatMessageResponse> GetMessages(int chatRoomId)
        {
            List<ChatMessage> messages = chatRepository.GetMessages(chatRoomId);

            List<ChatMessageResponse> result = new List<ChatMessageResponse>();

            foreach (ChatMessage message in messages)
            {
                result.Add(new ChatMessageResponse
                {
                    ChatMessageId = message.ChatMessageId,
                    ChatRoomId = message.ChatRoomId,
                    SenderId = message.SenderId,
                    SenderName = message.SenderName,

                    MessageType = message.MessageType,
                    Message = message.Message,

                    SpotId = message.SpotId,
                    SpotName = message.SpotName,
                    SpotAddress = message.SpotAddress,
                    SpotImageUrl = message.SpotImageUrl,

                    CreatedAt = message.CreatedAt
                });
            }

            return result;
        }

        public bool AddMessage(ChatMessageRequest request, out string message)
        {
            if (request.ChatRoomId <= 0)
            {
                message = "chatRoomId는 1 이상이어야 합니다.";
                return false;
            }

            if (request.SenderId <= 0)
            {
                message = "senderId는 로그인 성공 시 받은 회원 번호를 입력해야 합니다.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(request.Message))
            {
                message = "메시지를 입력하세요.";
                return false;
            }

            if (chatRepository.ExistsMember(request.SenderId) == false)
            {
                message = "존재하지 않는 회원입니다.";
                return false;
            }

            bool result = chatRepository.AddMessage(request);

            if (result == false)
            {
                message = "채팅 메시지 전송 실패";
                return false;
            }

            message = "채팅 메시지 전송 성공";
            return true;
        }

        public ShareTouristSpotResponse? ShareTouristSpot(ShareTouristSpotRequest request, out string message)
        {
            if (request.SenderId <= 0)
            {
                message = "senderId는 로그인 성공 시 받은 회원 번호를 입력해야 합니다.";
                return null;
            }

            if (request.SpotId <= 0)
            {
                message = "spotId는 1 이상이어야 합니다.";
                return null;
            }

            if (chatRepository.ExistsMember(request.SenderId) == false)
            {
                message = "존재하지 않는 회원입니다.";
                return null;
            }

            if (chatRepository.ExistsTouristSpot(request.SpotId) == false)
            {
                message = "존재하지 않는 관광지입니다.";
                return null;
            }

            ChatRoom? room = chatRepository.GetOrCreateShareRoom();

            if (room == null)
            {
                message = "채팅방 조회 실패";
                return null;
            }

            int chatMessageId = chatRepository.ShareTouristSpot(room.ChatRoomId, request);

            if (chatMessageId <= 0)
            {
                message = "관광지 공유 실패";
                return null;
            }

            message = "관광지 공유 성공";

            return new ShareTouristSpotResponse
            {
                ChatRoomId = room.ChatRoomId,
                ChatMessageId = chatMessageId
            };
        }
    }
}