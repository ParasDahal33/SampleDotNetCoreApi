using System.ComponentModel.DataAnnotations;

namespace BusReservationSystemApi.Data.DTO.Request
{
    public class UserLoginRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required!")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
        public string Password { get; set; }
    }
}
