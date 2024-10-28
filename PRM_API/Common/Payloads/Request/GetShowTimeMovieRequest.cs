using System.ComponentModel.DataAnnotations;

namespace PRM_API.Common.Payloads.Request;

public class GetShowTimeMovieRequest
{
    [Required(ErrorMessage = "MovieId is required!")]
    public int MovieId { get; set; }
}