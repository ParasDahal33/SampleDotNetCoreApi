namespace BusReservationSystemApi.Data.Configuration
{
    public class ServiceResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public IEnumerable<string> Errors { get; set; }
        public static ServiceResponse<T> Failed(string errorMessage, IEnumerable<string> error)
        {
            return new ServiceResponse<T> { Success = false, Message = errorMessage, Errors = error };
        }
        public static ServiceResponse<T> Succeeded(T data, string message)
        {
            return new ServiceResponse<T> { Success = true, Data = data, Message = message };
        }
    }

}
