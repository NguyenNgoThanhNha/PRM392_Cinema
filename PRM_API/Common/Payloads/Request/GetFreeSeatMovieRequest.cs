using System.ComponentModel.DataAnnotations;

namespace PRM_API.Common.Payloads.Request;

public class GetFreeSeatMovieRequest
{
    [Required(ErrorMessage = "HallId is required!")]
    public int hallId { get; set; }
}