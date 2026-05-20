namespace MyTourApi_Server.DTOs.Response
{
    public class ChatRoomResponse
    {
        public int ChatRoomId { get; set; }
        public string RoomName { get; set; } = "";
        public DateTime CreatedAt { get; set; }

    }
}
