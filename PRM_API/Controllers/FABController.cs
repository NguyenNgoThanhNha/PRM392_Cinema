using Microsoft.AspNetCore.Mvc;
using PRM_API.Services.Impl;

namespace PRM_API.Controllers;

[ApiController]
public class FABController(IFABService fabService) :ControllerBase
{
    
}