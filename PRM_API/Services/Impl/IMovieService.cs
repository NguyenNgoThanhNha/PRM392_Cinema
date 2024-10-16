using PRM_API.Dtos;
using PRM_API.Models;
using System.Linq.Expressions;

namespace PRM_API.Services.Impl
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDTO>> FindAllAsync();
        Task<IEnumerable<MovieDTO>> FindAllWithConditionAsync(
            Expression<Func<Movie, bool>>? filter,
            Func<IQueryable<Movie>, IOrderedQueryable<Movie>>? orderBy,
            string? includeProperties);
        Task<MovieDTO?> FindAsync(int movieId);
    }
}
