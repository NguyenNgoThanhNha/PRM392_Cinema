using PRM_API.Dtos;

namespace PRM_API.Common.Payloads.Response;

public class GetAllFABResponse
{
    public List<FoodBeverageDTO> fABList { get; set; }
}