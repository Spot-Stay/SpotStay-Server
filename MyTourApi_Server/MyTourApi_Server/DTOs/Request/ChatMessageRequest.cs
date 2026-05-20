namespace MyTourApi_Server.DTOs.Request
{
    public class ChatMessageRequest
    {
        public int ChatRoomId { get; set; }
        public int SenderId { get; set; }
        public string Message { get; set; } = "";

    }
}
