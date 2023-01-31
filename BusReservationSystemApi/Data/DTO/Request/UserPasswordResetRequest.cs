namespace BusReservationSystemApi.Data.DTO.Request
{
    public class UserPasswordResetRequest
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Id { get; set; }
        public string Token { get; set; }
    }
}
