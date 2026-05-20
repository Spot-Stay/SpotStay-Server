namespace MyTourApi_Server.Models
{
    public class ChatRoom
    {
        public int ChatRoomId { get; set; }
        public string RoomName { get; set; } = "";
        public DateTime CreatedAt { get; set; }

    }
}
