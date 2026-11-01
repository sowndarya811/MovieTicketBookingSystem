namespace MovieTicketBookingSystem.DTOs.Shows
{
    public class UpdateShowDto
    {
        public int MovieId { get; set; }

        public int ScreenId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal BasePrice { get; set; }

        public bool IsActive { get; set; }
    }
}