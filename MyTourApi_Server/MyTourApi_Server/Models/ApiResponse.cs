namespace MyTourApi_Server.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }    
        public string? Message { get; set; }  
        public T? Data { get; set; }           
        public int? TotalCount { get; set; } 

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