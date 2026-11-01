namespace MovieTicketBookingSystem.DTOs.Theatres
{
    public class UpdateTheatreDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? City { get; set; }

        public bool IsActive { get; set; }
    }
}