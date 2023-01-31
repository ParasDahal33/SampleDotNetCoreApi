namespace BusReservationSystemApi.Data.Configuration
{
    public class StatusString
    {

        public string Message { get; set; } = string.Empty;
        public bool Status { get; set; } = true;

        public static StatusString Succeeded(string message)
        {
            return new StatusString { Status = true, Message = message };
        }

        public static StatusString Failed(string message)
        {
            return new StatusString { Status = false, Message = message };
        }
    }
}
