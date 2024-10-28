using System;
using System.Collections.Generic;

namespace PRM_API.Models;

public partial class CinemaHall
{
    public int HallId { get; set; }

    public string HallName { get; set; } = null!;

    public string HallType { get; set; } = null!;

    public int TotalSeats { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
