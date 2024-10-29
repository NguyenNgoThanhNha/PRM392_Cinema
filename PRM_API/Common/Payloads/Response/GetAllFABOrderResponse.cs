using PRM_API.Common.Payloads.Request;

namespace PRM_API.Common.Payloads.Response;

public class GetAllFABOrderResponse
{
    public List<FABOrderDetails> listFABOrder { get; set; }
}

public class FABOrderDetails
{
    public int FoodId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }
    
    public int Amount { get; set; }
}