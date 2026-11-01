namespace MovieTicketBookingSystem.DTOs.Seats
{
    public class UpdateSeatDto
    {
        public int ScreenId { get; set; }

        public string SeatNumber { get; set; } = string.Empty;

        public string SeatType { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }
    }
}