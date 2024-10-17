﻿namespace PRM_API.Dtos
{
    public class MovieDTO
    {
        public int MovieId { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        public int? Duration { get; set; }

        public decimal? Rating { get; set; }

        public string? Genre { get; set; }

        public string? Language { get; set; }

        /*        public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();*/
    }
}
