namespace Ecommerce.DTOs.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<String> Errors { get; set; }

        public ApiResponse()
        {

            Errors = new List<String>();
        }
    }
}
