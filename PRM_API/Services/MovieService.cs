using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PRM_API.Dtos;
using PRM_API.Models;
using PRM_API.Repositories;
using PRM_API.Services.Impl;
using System.Linq.Expressions;

namespace PRM_API.Services
{
    public class MovieService : IMovieService
    {
        private IRepository<Movie, int> _repo;
        private readonly IMapper _mapper;

        public MovieService(IRepository<Movie, int> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MovieDTO>> FindAllAsync()
        {
            var movieEntities = await _repo.GetAll().Include(mv => mv.Showtimes).ToListAsync();
            return _mapper.Map<List<MovieDTO>>(movieEntities);
        }

        public async Task<IEnumerable<MovieDTO>> FindAllWithConditionAsync(
            Expression<Func<Movie, bool>>? filter, 
            Func<IQueryable<Movie>, IOrderedQueryable<Movie>>? orderBy, 
            string? includeProperties)
        {
            // Get all as Queryable 
            var query = _repo.GetAll();

            // Get data with filtering 
            if(filter != null) query = query.Where(filter);

            // Handle include table
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties.Split(',').ToArray())
                {
                    query.Include(includeProp);
                }
            }

            // Check whether order by or not 
            var movieEntities = orderBy != null
                ? await orderBy(query).ToListAsync()
                : await query.ToListAsync();

            return _mapper.Map<List<MovieDTO>>(movieEntities);
        }

        public async Task<MovieDTO?> FindAsync(int movieId)
        {
            var movieEntity = 
                await _repo.FindByCondition(x => x.MovieId == movieId)
                    .Include(x => x.Showtimes)
                    .FirstOrDefaultAsync();

            return _mapper.Map<MovieDTO?>(movieEntity);
        }
    }
}
