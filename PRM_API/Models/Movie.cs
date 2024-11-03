using System;
using System.Collections.Generic;

namespace PRM_API.Models;

public partial class Movie
{
    public int MovieId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public int? Duration { get; set; }

    public decimal? Rating { get; set; }

    public string? Genre { get; set; }

    public string? Language { get; set; }
    
    public string? LinkTrailer { get; set; }

    public string? PosterUrl { get; set; }

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
