using ClinicFlow_Backend.Data;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow_Backend.Repositories.Implementation
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClinicTask>> GetTasksByAssigneeAsync(Guid assignedTo)
        {
            return await _context.ClinicTasks
                .Where(t => t.AssignedTo == assignedTo)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<ClinicTask?> GetTaskAsync(Guid id)
        {
            return await _context.ClinicTasks.FindAsync(id);
        }

        public async Task<ClinicTask> PostTaskAsync(ClinicTask task)
        {
            _context.ClinicTasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task PutTaskAsync(Guid id, ClinicTask task)
        {
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
