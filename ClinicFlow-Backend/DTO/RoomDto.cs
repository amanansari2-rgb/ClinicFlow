namespace ClinicFlow_Backend.DTO
{
    // Returned by GET /rooms and GET /rooms/{id}
    public class RoomDto
    {
        public Guid RoomID { get; set; }
        public Guid ClinicID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? ResourcesJSON { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    // Used by POST /rooms
    public class CreateRoomDto
    {
        public Guid ClinicID { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; } = 1;
        public string? ResourcesJSON { get; set; }
    }

    // Used by PUT /rooms/{id}
    public class UpdateRoomDto
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? ResourcesJSON { get; set; }
        public string Status { get; set; } = string.Empty;  // Available | Occupied | Maintenance
    }
}