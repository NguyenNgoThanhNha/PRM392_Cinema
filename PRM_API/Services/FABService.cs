using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRM_API.Common.Payloads.Request;
using PRM_API.Common.Payloads.Response;
using PRM_API.Dtos;
using PRM_API.Exceptions;
using PRM_API.Models;
using PRM_API.Repositories;
using PRM_API.Services.Impl;

namespace PRM_API.Services;

public class FABService :IFABService
{
    private readonly IRepository<FoodBeverage, int> _fAbRepository;
    private readonly IMapper _mapper;
    private readonly IRepository<BookingFoodBeverage, int> _fabOrderRepository;

    public FABService(IRepository<FoodBeverage,int> fAbRepository,IMapper mapper,
        IRepository<BookingFoodBeverage,int> fabOrderRepository)
    {
        _fAbRepository = fAbRepository;
        _mapper = mapper;
        _fabOrderRepository = fabOrderRepository;
    }
    public async Task<IEnumerable<FoodBeverageDTO>> GetAllFAB()
    {
        var result =await _fAbRepository.GetAll().ToListAsync();
        if (!result.Any()) throw new NotFoundException("There is no FAB yet");
        return _mapper.Map<List<FoodBeverageDTO>>(result);
    }

    public async Task<FoodBeverageDTO> GetFABWithId(int id)
    {
        var result =await _fAbRepository.FindByCondition(fab => fab.FoodId == id).FirstOrDefaultAsync();
        if (result is null) throw new NotFoundException("There is no FAB matched");
        return _mapper.Map<FoodBeverageDTO>(result);
    }

    public async Task CreateFABOrder(int orderId,CreateFABOrderRequest req)
    {
        var addingList = new List<BookingFoodBeverage>();
        foreach (var f in req.listFABOrder)
        {
            var order = new BookingFoodBeverage()
            {
                FoodId = f.fABId,
                Quantity = f.amount,
                BookingId = orderId,
            };
            addingList.Add(order);
        }

        await _fabOrderRepository.AddRangeAsync(addingList);
    }

    public async Task<List<FABOrderDetails>> GetAllFABOrderResponse(int orderId)
    {
        var result = await _fabOrderRepository.FindByCondition(o => o.BookingId == orderId)
            .Include(o => o.Food)
            .Select(o => new FABOrderDetails()
            {
                FoodId = o.FoodId,
                Name = o.Food.Name,
                Price = o.Food.Price,
                Description = o.Food.Description,
                Amount = o.Quantity
            }).ToListAsync();
        return result;
    }

    public async Task<bool> DeleteFABOrder(int orderId)
    {
        _fabOrderRepository.Remove(orderId);
        bool result = await _fAbRepository.Commit() > 0 ? true : false;
        return result;
    }

    public async Task<bool> UpdateFABOrder(int orderId,UpdateBookingFABRequest req)
    {
        var updateFoodIds = req.UpdateFABOrders.Select(o => o.fABId).ToList();
        var existedFoodIds = await _fabOrderRepository.FindByCondition(o => o.BookingId == orderId)
            .Select(o => o.FoodId)
            .ToListAsync();
        var foodIdsToRemove = existedFoodIds.Except(updateFoodIds).ToList();
        var foodIdsToAdd = updateFoodIds.Except(existedFoodIds).ToList();
        var foodIdsUpdateRemaining = existedFoodIds
            .Except(foodIdsToRemove)
            .ToList();
        foreach (int id in foodIdsToRemove)
        {
            var deleteObject = await _fabOrderRepository.FindByCondition(o => o.BookingId == orderId
                                                                              && o.FoodId == id).FirstAsync();
            _fabOrderRepository.Remove(deleteObject.BookingFoodId);
        }

        foreach (int id in foodIdsToAdd)
        {
            var foodOrder = new BookingFoodBeverage()
            {
                FoodId = id,
                Quantity = req.UpdateFABOrders.Where(o => o.fABId == id).Select(o =>o.amount).First(),
                BookingId = orderId
            };
            await _fabOrderRepository.AddAsync(foodOrder);
        }

        foreach (int id in foodIdsUpdateRemaining)
        {
            var existedObject = await _fabOrderRepository.FindByCondition(o => o.BookingId == orderId
                                                                               && o.FoodId == id).FirstAsync();
            existedObject.Quantity = req.UpdateFABOrders.Where(o => o.fABId == id).Select(o => o.amount).First(); 
            _fabOrderRepository.Update(existedObject);
        }

        return await _fabOrderRepository.Commit() > 0 ? true : false;
    }
}