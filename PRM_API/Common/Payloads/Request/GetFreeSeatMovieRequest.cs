using System.ComponentModel.DataAnnotations;

namespace PRM_API.Common.Payloads.Request;

public class GetFreeSeatMovieRequest
{
    [Required(ErrorMessage = "ShowTimeId is required!")]
    public int showTimeId { get; set; }
}