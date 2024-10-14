using System;
using System.Collections.Generic;

namespace PRM_API.Models;

public partial class Seat
{
    public int SeatId { get; set; }

    public int HallId { get; set; }

    public string SeatNumber { get; set; } = null!;

    public string SeatType { get; set; } = null!;

    public virtual ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();

    public virtual CinemaHall Hall { get; set; } = null!;
}
