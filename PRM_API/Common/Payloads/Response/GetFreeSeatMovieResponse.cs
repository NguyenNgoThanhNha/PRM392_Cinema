using PRM_API.Dtos;

namespace PRM_API.Common.Payloads.Response;

public class GetFreeSeatMovieResponse
{
    public List<SeatDTO> data { get; set; }
}