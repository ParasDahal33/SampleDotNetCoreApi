namespace BusReservationSystemApi.Data.DTO.Request
{
    public class ExtendPasswordExpiryRequest
    {
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
