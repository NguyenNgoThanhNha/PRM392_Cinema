using System;
using System.Collections.Generic;

namespace PRM_API.Models;

public partial class BookingFoodBeverage
{
    public int BookingFoodId { get; set; }

    public int BookingId { get; set; }

    public int FoodId { get; set; }

    public int Quantity { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual FoodBeverage Food { get; set; } = null!;
}
