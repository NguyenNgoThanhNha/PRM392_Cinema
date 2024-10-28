namespace PRM_API.Dtos
{
    public class ShowtimeDTO
    {
        public int ShowtimeId { get; set; }

        public int MovieId { get; set; }

        public int HallId { get; set; }

        public decimal SeatPrice { get; set; }

        public DateTime ShowDate { get; set; }

        /*        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();*/

        public virtual HallDTO? Hall { get; set; }

        public virtual MovieDTO? Movie { get; set; }
    }
}
