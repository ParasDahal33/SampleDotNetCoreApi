using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusReservationSystemApi.Data.Models
{
    public class UserToken
    {
        [Key, Column(Order = 0)]
        public string UserId { get; set; }
        [Key, Column(Order = 1)]
        public string UserRefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
