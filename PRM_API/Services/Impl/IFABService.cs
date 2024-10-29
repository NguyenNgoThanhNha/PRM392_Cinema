using PRM_API.Common.Payloads.Request;
using PRM_API.Common.Payloads.Response;
using PRM_API.Dtos;

namespace PRM_API.Services.Impl;

public interface IFABService
{
    Task<IEnumerable<FoodBeverageDTO>> GetAllFAB();
    Task<FoodBeverageDTO> GetFABWithId(int id);
    Task CreateFABOrder(int orderId, CreateFABOrderRequest req);
    Task<List<FABOrderDetails>> GetAllFABOrderResponse(int orderId);
    Task<bool> DeleteFABOrder(int orderId);
    Task<bool> UpdateFABOrder(int orderId, UpdateBookingFABRequest req);

}