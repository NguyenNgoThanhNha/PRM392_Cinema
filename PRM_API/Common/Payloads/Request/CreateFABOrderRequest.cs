namespace PRM_API.Common.Payloads.Request;

public class CreateFABOrderRequest
{
    public List<FABOrder> listFABOrder { get; set; }
}

public class FABOrder
{
    public int fABId { get; set; }
    public int amount { get; set; }
}