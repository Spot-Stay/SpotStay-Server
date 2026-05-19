namespace MyTourApi_Server.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }      // 성공 여부
        public string? Message { get; set; }    // 메시지
        public T? Data { get; set; }            // 실제 데이터
        public int? TotalCount { get; set; }   // 전체 개수 (리스트일 때)

        // 성공 응답 만들기
        public static ApiResponse<T> Ok(T data, string message = "성공", int? totalCount = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                TotalCount = totalCount
            };
        }

        // 실패 응답 만들기
        public static ApiResponse<T> Fail(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }
    }
}