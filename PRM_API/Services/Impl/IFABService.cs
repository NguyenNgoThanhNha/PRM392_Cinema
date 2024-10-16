using PRM_API.Dtos;

namespace PRM_API.Services.Impl;

public interface IFABService
{
    Task<IEnumerable<FoodBeverageDTO>> GetAllFAB();
    Task<FoodBeverageDTO> GetFABWithId(int id);
}