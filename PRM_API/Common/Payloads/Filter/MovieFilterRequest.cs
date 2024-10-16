namespace PRM_API.Common.Payloads.Filter
{
    public class MovieFilterRequest
    {
        public string? Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        public int? Duration { get; set; }

        public decimal? Rating { get; set; }

        public string? Genre { get; set; }

        public string? Language { get; set; }

        public DateTime? ShowTimeFrom { get; set; }
        public DateTime? ShowTimeTo { get; set; }
    }
}
