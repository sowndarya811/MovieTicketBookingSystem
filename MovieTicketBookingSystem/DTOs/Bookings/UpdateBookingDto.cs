namespace MovieTicketBookingSystem.DTOs.Bookings
{
    public class UpdateBookingDto
    {
        public int ShowId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerPhone { get; set; } = string.Empty;

        public string Status { get; set; } = "Confirmed";
    }
}