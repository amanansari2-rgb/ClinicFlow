namespace ClinicFlow_Backend.DTO
{
    // Returned by GET /api/v1/notifications
    public class NotificationDto
    {
        public Guid      NotificationID { get; set; }
        public Guid      UserID         { get; set; }
        public string?   EntityID       { get; set; }
        public string    Message        { get; set; } = string.Empty;
        public string    Category       { get; set; } = string.Empty;
        public string    Severity       { get; set; } = string.Empty;
        public DateTime  CreatedAt      { get; set; }
        public DateTime? ReadAt         { get; set; }
        public string    Status         { get; set; } = string.Empty;
    }
}
