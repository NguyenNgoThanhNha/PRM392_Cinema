namespace PRM_API.Dtos;

public class BookingDTO
{
    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int ShowtimeId { get; set; }

    public DateTime BookingDate { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; }

    /*    public virtual ICollection<BookingFoodBeverage>? BookingFoodBeverages { get; set; }

        public virtual ICollection<BookingSeat>? BookingSeats { get; set; } = new List<BookingSeat>();*/

    public virtual ShowtimeDTO? Showtime { get; set; }

    public virtual UserDTO? User { get; set; }
}