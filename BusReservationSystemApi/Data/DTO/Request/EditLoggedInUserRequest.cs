namespace BusReservationSystemApi.Data.DTO.Request
{
    public class EditLoggedInUserRequest
    {

        public string FullName { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
    }
}
