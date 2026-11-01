namespace MovieTicketBookingSystem.DTOs.Bookings
{
    public class CreateBookingDto
    {
        public int ShowId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerPhone { get; set; } = string.Empty;

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Confirmed";
    }
}