using BusReservationSystemApi.Data.Enumeration;
using System.ComponentModel.DataAnnotations;

namespace BusReservationSystemApi.Data.DTO.Request
{
    public class UserEditRequest
    {

        [Required(ErrorMessage = "Id is required.")]
        public string Id { get; set; }
        public string FullName { get; init; } = string.Empty;
        public string UserName { get; init; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public UserStatus UserStatus { get; init; } = UserStatus.Active;
    }
}
