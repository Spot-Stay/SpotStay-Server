namespace MyTourApi_Server.Models
{
    public class ChatMessage
    {
        public int ChatMessageId { get; set; }
        public int ChatRoomId { get; set; }

        public int SenderId { get; set; }
        public string? SenderName { get; set; }

        public string MessageType { get; set; } = "";
        public string? Message { get; set; }

        public int? SpotId { get; set; }
        public string? SpotName { get; set; }
        public string? SpotAddress { get; set; }
        public string? SpotImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
