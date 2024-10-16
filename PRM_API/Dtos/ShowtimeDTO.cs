using PRM_API.Models;
using System.Text.Json.Serialization;

namespace PRM_API.Dtos
{
    public class ShowtimeDTO
    {
        public int ShowtimeId { get; set; }

        public int MovieId { get; set; }

        public int HallId { get; set; }

        public DateTime ShowDate { get; set; }

        /// <summary>
        /// Ae nao` viet phan nay` viet DTO cho no r bo cmt nh
        /// </summary>
        //public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        //public virtual CinemaHall Hall { get; set; } = null!;

        [JsonIgnore]
        public virtual MovieDTO Movie { get; set; } = null!;
    }
}
