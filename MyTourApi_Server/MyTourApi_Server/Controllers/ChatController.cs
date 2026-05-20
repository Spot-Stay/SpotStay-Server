using Microsoft.AspNetCore.Mvc;
using MyTourApi_Server.DTOs.Request;
using MyTourApi_Server.DTOs.Response;
using MyTourApi_Server.Models;
using MyTourApi_Server.Services.Interfaces;

namespace MyTourApi_Server.Controllers
{
    [ApiController]
    [Route("api/chats")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService chatService;

        public ChatController(IChatService chatService)
        {
            this.chatService = chatService;
        }

        // 채팅방 열기
        [HttpGet("room")]
        public IActionResult GetRoom()
        {
            ChatRoomResponse? result = chatService.GetOrCreateShareRoom();

            if (result == null)
            {
                return Ok(ApiResponse<object>.Fail("채팅방 조회 실패"));
            }

            return Ok(ApiResponse<ChatRoomResponse>.Ok(
                result,
                "채팅방 조회 성공"
            ));
        }

        // 채팅 메시지 조회
        [HttpGet("rooms/{chatRoomId}/messages")]
        public IActionResult GetMessages(int chatRoomId)
        {
            if (chatRoomId <= 0)
            {
                return Ok(ApiResponse<object>.Fail("chatRoomId는 1 이상이어야 합니다."));
            }

            List<ChatMessageResponse> result = chatService.GetMessages(chatRoomId);

            return Ok(ApiResponse<List<ChatMessageResponse>>.Ok(
                result,
                "채팅 메시지 조회 성공",
                result.Count
            ));
        }

        // 일반 채팅 메시지 전송
        [HttpPost("messages")]
        public IActionResult AddMessage([FromBody] ChatMessageRequest request)
        {
            bool result = chatService.AddMessage(request, out string message);

            if (result == false)
            {
                return Ok(ApiResponse<object>.Fail(message));
            }

            return Ok(ApiResponse<object>.Ok(
                null,
                message
            ));
        }

        // 관광지 공유
        [HttpPost("share-tourist-spot")]
        public IActionResult ShareTouristSpot([FromBody] ShareTouristSpotRequest request)
        {
            ShareTouristSpotResponse? result = chatService.ShareTouristSpot(request, out string message);

            if (result == null)
            {
                return Ok(ApiResponse<object>.Fail(message));
            }

            return Ok(ApiResponse<ShareTouristSpotResponse>.Ok(
                result,
                message
            ));
        }
    }
}