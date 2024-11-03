using Microsoft.AspNetCore.Mvc;
using PRM_API.Common.Enum;
using PRM_API.Common.Payloads;
using PRM_API.Common.Payloads.Filter;
using PRM_API.Dtos;
using PRM_API.Services.Impl;

namespace PRM_API.Controllers
{
    [ApiController]
    public class MovieController(IMovieService movieService) : ControllerBase
    {

        [HttpGet(ApiRoute.Movie.GetAll, Name = nameof(GetAllMovieAsync))]
        public async Task<IActionResult> GetAllMovieAsync([FromQuery] MovieFilterRequest? filter)
        {
            // Initiate list of movie
            List<MovieDTO> movies = new();
            // Check whether get by user or not 
            if (!HasFilterCriteria(filter!))
            {
                movies = (await movieService.FindAllAsync()).ToList();
            }
            else
            {
                movies = (await movieService.FindAllWithConditionAsync(
                    filter: mv => mv.Title.Contains(filter!.Title!) ||
                        (!string.IsNullOrEmpty(mv.Description)
                            && mv.Description.Contains(filter.Description!)) ||
                        (mv.ReleaseDate.HasValue && filter.ReleaseDate.HasValue
                            && mv.ReleaseDate.Value.Equals(filter.ReleaseDate.Value)) ||
                        (mv.Duration.HasValue && mv.Duration.Value == filter.Duration) ||
                        (mv.Rating.HasValue && mv.Rating.Value == filter.Rating) || 
                        ((!string.IsNullOrEmpty(mv.Genre) && mv.Genre == filter.Genre) && 
                        (!string.IsNullOrEmpty(mv.Language) && mv.Language == filter.Language)) ||
                        (mv.Showtimes.Any() && mv.Showtimes.Any(st => 
                            st.ShowDate >= filter.ShowTimeFrom && st.ShowDate <= filter.ShowTimeTo)),
                    orderBy: null,
                    includeProperties: "Showtimes"
                    )).ToList();
            }

            return Ok(movies);
        }
        private bool HasFilterCriteria(MovieFilterRequest filter)
        {
            if(filter == null) return false;
            return !string.IsNullOrEmpty(filter.Title) ||
                   !string.IsNullOrEmpty(filter.Description) ||
                   filter.ReleaseDate.HasValue ||
                   filter.Duration.HasValue ||
                   filter.Rating.HasValue ||
                   !string.IsNullOrEmpty(filter.Genre) ||
                   !string.IsNullOrEmpty(filter.Language) ||
                   filter.ShowTimeFrom.HasValue ||
                   filter.ShowTimeTo.HasValue;
        }

        [HttpGet(ApiRoute.Movie.GetById, Name = nameof(GetMovieByIdAsync))]
        public async Task<IActionResult> GetMovieByIdAsync([FromRoute] int id)
        {
            var movieDTO = await movieService.FindAsync(id);
            return Ok(movieDTO);
        }

        [HttpGet(ApiRoute.Movie.GetAllGenre, Name = nameof(GetAllMovieGenreAsync))]
        public async Task<IActionResult> GetAllMovieGenreAsync()
        {
            List<string> movieGenres = new()
            {
                nameof(MovieGenre.Action),
                nameof(MovieGenre.Adventure),
                nameof(MovieGenre.Drama),
                nameof(MovieGenre.Fantasy),
                nameof(MovieGenre.Horror),
                nameof(MovieGenre.Mystery),
                nameof(MovieGenre.Romance),
                nameof(MovieGenre.ScienceFiction),
                nameof(MovieGenre.Thriller),
                nameof(MovieGenre.Animation),
            };

            return await Task.FromResult(Ok(movieGenres));
        }

        [HttpGet(ApiRoute.Movie.GetAllLang, Name = nameof(GetAllMovieLanguageAsync))]
        public async Task<IActionResult> GetAllMovieLanguageAsync()
        {
            List<string> movieLangs = new()
            {
                nameof(MovieLanguage.English),
                nameof(MovieLanguage.Spanish),
                nameof(MovieLanguage.French),
                nameof(MovieLanguage.German),
                nameof(MovieLanguage.Japanese),
                nameof(MovieLanguage.Korean),
                nameof(MovieLanguage.Thai),
                nameof(MovieLanguage.Vietnamese),
            };

            return await Task.FromResult(Ok(movieLangs));
        }
    }
}
