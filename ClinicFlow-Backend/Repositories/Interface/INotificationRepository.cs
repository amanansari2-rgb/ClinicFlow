using ClinicFlow_Backend.Model;

namespace ClinicFlow_Backend.Repositories.Interface
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetNotificationsByUserAsync(Guid userId);
        Task<Notification?> GetNotificationAsync(Guid id);
        Task PutNotificationAsync(Guid id, Notification notification);
    }
}
