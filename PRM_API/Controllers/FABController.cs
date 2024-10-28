using Microsoft.AspNetCore.Mvc;
using PRM_API.Common.Payloads;
using PRM_API.Common.Payloads.Request;
using PRM_API.Common.Payloads.Response;
using PRM_API.Dtos;
using PRM_API.Exceptions;
using PRM_API.Services.Impl;

namespace PRM_API.Controllers;

[ApiController]
public class FABController(IFABService fabService) :ControllerBase
{
    [HttpGet(ApiRoute.FAB.GetAll, Name = nameof(GetAllFABAsync))]
    public async Task<IActionResult> GetAllFABAsync()
    {
        var result = await fabService.GetAllFAB();
        return Ok(ApiResult<GetAllFABResponse>.Succeed(new GetAllFABResponse()
        {
            fABList = result.ToList()
        }));
    }

    [HttpGet(ApiRoute.FAB.GetById, Name = nameof(GetById))]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var result = await fabService.GetFABWithId(id);
        return Ok(ApiResult<FoodBeverageDTO>.Succeed(result));
    }

    [HttpPost(ApiRoute.FAB.CreateFABOrder)]
    public async Task<IActionResult> CreateFABOrder([FromRoute] int orderId, [FromBody] CreateFABOrderRequest req)
    {
        await fabService.CreateFABOrder(orderId, req);
        return Ok(ApiResult<object>.Succeed(new 
        {
            Message = "Order FAB successfully"
        }));
    }

    [HttpGet(ApiRoute.FAB.GetByBookingId)]
    public async Task<IActionResult> GetByBookingId([FromRoute] int orderId)
    {
        var result = await fabService.GetAllFABOrderResponse(orderId);
        if (!result.Any()) throw new BadRequestException("There is no food or drink added");
        return Ok(ApiResult<GetAllFABOrderResponse>.Succeed(new GetAllFABOrderResponse()
        {
            listFABOrder = result
        }));
    }

    [HttpDelete(ApiRoute.FAB.DeleteFABOrder)]
    public async Task<IActionResult> Delete([FromRoute] int orderId, [FromBody] UpdateBookingFABRequest req)
    {
        bool result = await fabService.UpdateFABOrder(orderId, req);
        if (!result) throw new BadRequestException("Nothing to update or something went wrong");
        return Ok(ApiResult<object>.Succeed(new
        {
            Message = "Update Successfully"
        }));
    }
    
}