namespace MovieTicketBookingSystem.Models
{
    public class Seat
    {
        public int SeatId { get; set; }

        public int ScreenId { get; set; }

        public string SeatNumber { get; set; } = string.Empty;

        public string SeatType { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;
    }
}