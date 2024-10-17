using PRM_API.Dtos;

namespace PRM_API.Common.Payloads.Response;

public class GetShowTimeMovieResponse
{
    public List<ShowtimeDTO> data { get; set; }
}