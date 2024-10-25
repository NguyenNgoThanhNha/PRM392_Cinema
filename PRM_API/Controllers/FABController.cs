using Microsoft.AspNetCore.Mvc;
using PRM_API.Common.Payloads;
using PRM_API.Common.Payloads.Response;
using PRM_API.Dtos;
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
    
    

}