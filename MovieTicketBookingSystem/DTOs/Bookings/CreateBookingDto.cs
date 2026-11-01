namespace MovieTicketBookingSystem.DTOs.Bookings
{
    public class CreateBookingDto
    {
        public int ShowId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerPhone { get; set; } = string.Empty;

        public List<int> SeatIds { get; set; } = new();
    }
}