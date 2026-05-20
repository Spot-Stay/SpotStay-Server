using Microsoft.Data.SqlClient;
using MyTourApi_Server.Models;
using MyTourApi_Server.DTOs.Request;
using MyTourApi_Server.Repositories.Interfaces;

namespace MyTourApi_Server.Repositories.Impls
{
    public class ChatRepository : IChatRepository
    {
        private readonly Db db;

        public ChatRepository(Db db)
        {
            this.db = db;
        }

        public ChatRoom? GetOrCreateShareRoom()
        {
            string sql = @"
IF NOT EXISTS (SELECT 1 FROM dbo.ChatRoom WHERE RoomName = N'관광지 공유 채팅방')
BEGIN
    INSERT INTO dbo.ChatRoom(RoomName)
    VALUES(N'관광지 공유 채팅방');
END

SELECT ChatRoomId, RoomName, CreatedAt
FROM dbo.ChatRoom
WHERE RoomName = N'관광지 공유 채팅방';";

            using SqlConnection conn = db.GetConnection();
            using SqlCommand cmd = new SqlCommand(sql, conn);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new ChatRoom
                {
                    ChatRoomId = Convert.ToInt32(reader["ChatRoomId"]),
                    RoomName = reader["RoomName"].ToString() ?? "",
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                };
            }

            return null;
        }

        public List<ChatMessage> GetMessages(int chatRoomId)
        {
            string sql = @"
SELECT
    CM.ChatMessageId,
    CM.ChatRoomId,
    CM.SenderId,
    M.Name AS SenderName,
    CM.MessageType,
    CM.Message,
    CM.SpotId,
    TS.Name AS SpotName,
    TS.Address AS SpotAddress,
    TS.ImageUrl AS SpotImageUrl,
    CM.CreatedAt
FROM dbo.ChatMessage CM
INNER JOIN dbo.Member M
    ON CM.SenderId = M.MemberId
LEFT JOIN dbo.TouristSpot TS
    ON CM.SpotId = TS.SpotId
WHERE CM.ChatRoomId = @ChatRoomId
ORDER BY CM.CreatedAt ASC";

            List<ChatMessage> list = new List<ChatMessage>();

            using SqlConnection conn = db.GetConnection();
            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@ChatRoomId", chatRoomId);

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(ReadChatMessage(reader));
            }

            return list;
        }

        public bool AddMessage(ChatMessageRequest request)
        {
            string sql = @"
INSERT INTO dbo.ChatMessage(ChatRoomId, SenderId, MessageType, Message, SpotId)
VALUES(@ChatRoomId, @SenderId, 'TEXT', @Message, NULL)";

            using SqlConnection conn = db.GetConnection();
            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@ChatRoomId", request.ChatRoomId);
            cmd.Parameters.AddWithValue("@SenderId", request.SenderId);
            cmd.Parameters.AddWithValue("@Message", request.Message);

            conn.Open();

            int result = cmd.ExecuteNonQuery();

            return result > 0;
        }

        public int ShareTouristSpot(int chatRoomId, ShareTouristSpotRequest request)
        {
            string sql = @"
INSERT INTO dbo.ChatMessage(ChatRoomId, SenderId, MessageType, Message, SpotId)
OUTPUT INSERTED.ChatMessageId
SELECT
    @ChatRoomId,
    @SenderId,
    'SPOT',
    N'[관광지 공유] ' + Name,
    SpotId
FROM dbo.TouristSpot
WHERE SpotId = @SpotId";

            using SqlConnection conn = db.GetConnection();
            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@ChatRoomId", chatRoomId);
            cmd.Parameters.AddWithValue("@SenderId", request.SenderId);
            cmd.Parameters.AddWithValue("@SpotId", request.SpotId);

            conn.Open();

            object? result = cmd.ExecuteScalar();

            if (result == null || result == DBNull.Value)
                return 0;

            return Convert.ToInt32(result);
        }

        public bool ExistsMember(int memberId)
        {
            string sql = @"
SELECT COUNT(*)
FROM dbo.Member
WHERE MemberId = @MemberId";

            using SqlConnection conn = db.GetConnection();
            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@MemberId", memberId);

            conn.Open();

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }

        public bool ExistsTouristSpot(int spotId)
        {
            string sql = @"
SELECT COUNT(*)
FROM dbo.TouristSpot
WHERE SpotId = @SpotId";

            using SqlConnection conn = db.GetConnection();
            using SqlCommand cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@SpotId", spotId);

            conn.Open();

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }

        private ChatMessage ReadChatMessage(SqlDataReader reader)
        {
            return new ChatMessage
            {
                ChatMessageId = Convert.ToInt32(reader["ChatMessageId"]),
                ChatRoomId = Convert.ToInt32(reader["ChatRoomId"]),
                SenderId = Convert.ToInt32(reader["SenderId"]),
                SenderName = reader["SenderName"] == DBNull.Value ? null : reader["SenderName"].ToString(),

                MessageType = reader["MessageType"].ToString() ?? "",
                Message = reader["Message"] == DBNull.Value ? null : reader["Message"].ToString(),

                SpotId = reader["SpotId"] == DBNull.Value ? null : Convert.ToInt32(reader["SpotId"]),
                SpotName = reader["SpotName"] == DBNull.Value ? null : reader["SpotName"].ToString(),
                SpotAddress = reader["SpotAddress"] == DBNull.Value ? null : reader["SpotAddress"].ToString(),
                SpotImageUrl = reader["SpotImageUrl"] == DBNull.Value ? null : reader["SpotImageUrl"].ToString(),

                CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
            };
        }
    }
}