using System;
using System.Collections.Generic;

namespace PRM_API.Models;

public partial class FoodBeverage
{
    public int FoodId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<BookingFoodBeverage> BookingFoodBeverages { get; set; } = new List<BookingFoodBeverage>();
}
