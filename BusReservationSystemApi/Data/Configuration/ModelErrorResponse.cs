namespace BusReservationSystemApi.Data.Configuration
{
    public class ModelErrorResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
        public bool Success { get; set; } = false;

    }

    public class ErrorModel
    {
        public string FieldName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
