using ClinicFlow_Backend.Model;

namespace ClinicFlow_Backend.Repositories.Interface
{
    public interface ITaskRepository
    {
        Task<IEnumerable<ClinicTask>> GetTasksByAssigneeAsync(Guid assignedTo);
        Task<ClinicTask?> GetTaskAsync(Guid id);
        Task<ClinicTask> PostTaskAsync(ClinicTask task);
        Task PutTaskAsync(Guid id, ClinicTask task);
    }
}
