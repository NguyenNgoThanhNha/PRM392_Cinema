using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

    public FABService(IRepository<FoodBeverage,int> fAbRepository,IMapper mapper)
    {
        _fAbRepository = fAbRepository;
        _mapper = mapper;
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
}