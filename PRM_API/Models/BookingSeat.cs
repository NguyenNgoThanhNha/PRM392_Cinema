using System;
using System.Collections.Generic;

namespace PRM_API.Models;

public partial class BookingSeat
{
    public int BookingSeatId { get; set; }

    public int? BookingId { get; set; }

    public int SeatId { get; set; }

    public string BookingSeatStatus { get; set; } = null!;

    public virtual Booking? Booking { get; set; }

    public virtual Seat Seat { get; set; } = null!;
}
