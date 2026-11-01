namespace MovieTicketBookingSystem.DTOs.Screens
{
    public class UpdateScreenDto
    {
        public int TheatreId { get; set; }

        public string Name { get; set; } = string.Empty;

        public int TotalSeats { get; set; }

        public bool IsActive { get; set; }
    }
}