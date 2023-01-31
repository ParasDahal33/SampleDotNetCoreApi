using System.ComponentModel.DataAnnotations;

namespace BusReservationSystemApi.Data.DTO.Request
{
    public class UserRegisterRequest
    {


        [Required(ErrorMessage = "Full name is required.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }
        public string UserType { get; set; }
        public int? ClientId { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }

    }
}
