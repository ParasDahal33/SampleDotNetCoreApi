namespace BusReservationSystemApi.Data.DTO.Response
{
    public class UserDataResponse
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public int UserStatus { get; set; } = 0;
        public bool EmailConfirmed { get; set; } = false;
        public DateTime PasswordChangeDate { get; set; } = DateTime.MinValue;
        public DateTime AccountCreatedDate { get; set; } = DateTime.MinValue;
        public DateTime ExpiryDate { get; set; } = DateTime.MinValue;
        public DateTime PwdExpiry { get; set; } = DateTime.MinValue;
        public string Role { get; set; } = string.Empty;

    }
}
