namespace PRM_API.Dtos;

public class BookingFoodBeverageDTO
{
    public int BookingFoodId { get; set; }

    public int BookingId { get; set; }

    public int FoodId { get; set; }

    public int Quantity { get; set; }

    public virtual BookingDTO Booking { get; set; } = null!;

    public virtual FoodBeverageDTO Food { get; set; } = null!;
}