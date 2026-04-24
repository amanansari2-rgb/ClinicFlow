namespace ClinicFlow_Backend.DTO
{
    // Returned by GET /api/v1/tasks
    public class TaskDto
    {
        public Guid      TaskID          { get; set; }
        public Guid      AssignedTo      { get; set; }
        public string?   RelatedEntityID { get; set; }
        public string    Description     { get; set; } = string.Empty;
        public DateTime  DueDate         { get; set; }
        public string    Priority        { get; set; } = string.Empty;
        public DateTime  CreatedAt       { get; set; }
        public DateTime? CompletedAt     { get; set; }
        public string    Status          { get; set; } = string.Empty;
    }

    // Used by POST /api/v1/tasks
    public class CreateTaskDto
    {
        public Guid     AssignedTo      { get; set; }
        public string   Description     { get; set; } = string.Empty;
        public DateTime DueDate         { get; set; }
        public string?  RelatedEntityID { get; set; }
        public string   Priority        { get; set; } = "Medium";
    }

    // Used by PUT /api/v1/tasks/{id}
    public class UpdateTaskDto
    {
        public Guid      AssignedTo      { get; set; }
        public string    Description     { get; set; } = string.Empty;
        public DateTime  DueDate         { get; set; }
        public string    Priority        { get; set; } = string.Empty;
        public string    Status          { get; set; } = string.Empty;
        public string?   RelatedEntityID { get; set; }
        public DateTime? CompletedAt     { get; set; }
    }
}
