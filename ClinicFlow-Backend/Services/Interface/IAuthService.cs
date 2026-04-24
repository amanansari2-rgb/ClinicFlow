using ClinicFlow_Backend.DTO;

namespace ClinicFlow_Backend.Services.Interface
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
        Task<LoginResponseDto> RegisterAsync(RegisterRequestDto dto);
        Task LogoutAsync();
    }
}
