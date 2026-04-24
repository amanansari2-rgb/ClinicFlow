using ClinicFlow_Backend.Data;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
namespace ClinicFlow_Backend.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.Where(u => u.Status != "Inactive").ToListAsync();
        }

        public async Task<User?> GetUserAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserID == id && u.Status != "Inactive");
        }

        public async Task<bool> PutUserAsync(Guid id, User user)
        {
            if (id != user.UserID)
                return false;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExistsAsync(id))
                    return false;

                throw;
            }
        }

        public async Task<User> PostUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == id && u.Status != "Inactive");
            if (user == null)
                return false;

            user.Status = "Inactive";
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> UserExistsAsync(Guid id)
        {
            return await _context.Users.AnyAsync(e => e.UserID == id);
        }
    }
}