namespace MovieTicketBookingSystem.DTOs.Movies
{
    public class CreateMovieDto
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int DurationMinutes { get; set; }

        public string? Language { get; set; }

        public string? Genre { get; set; }

        public DateTime ReleaseDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}