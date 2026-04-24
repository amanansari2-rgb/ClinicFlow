namespace ClinicFlow_Backend.DTO
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
