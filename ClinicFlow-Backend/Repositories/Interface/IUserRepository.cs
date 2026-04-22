using ClinicFlow_Backend.Model;
namespace ClinicFlow_Backend.Repositories.Interface
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> GetUserAsync(Guid id);
        Task<bool> PutUserAsync(Guid id, User user);
        Task<User> PostUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid id);
    }
}