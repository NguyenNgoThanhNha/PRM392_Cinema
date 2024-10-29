namespace PRM_API.Dtos;

public class FoodBeverageDTO
{
    public int FoodId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<BookingFoodBeverageDTO> BookingFoodBeverages { get; set; } = new List<BookingFoodBeverageDTO>();

}