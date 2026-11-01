namespace MovieTicketBookingSystem.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int ShowId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerPhone { get; set; } = string.Empty;

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Confirmed";
    }
}