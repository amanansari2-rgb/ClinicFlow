using ClinicFlow_Backend.Data;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow_Backend.Repositories.Implementation
{
    public class SchedulingRepository : ISchedulingRepository
    {
        private readonly AppDbContext _context;

        public SchedulingRepository(AppDbContext context)
        {
            _context = context;
        }

        // ── Provider ──────────────────────────────────────────────────────────

        public async Task<IEnumerable<Provider>> GetProvidersAsync()
            => await _context.Providers.ToListAsync();

        public async Task<Provider?> GetProviderAsync(Guid id)
            => await _context.Providers.FindAsync(id);

        public async Task<Provider> AddProviderAsync(Provider provider)
        {
            _context.Providers.Add(provider);
            await _context.SaveChangesAsync();
            return provider;
        }

        public async Task<bool> UpdateProviderAsync(Guid id, Provider provider)
        {
            if (id != provider.ProviderID) return false;
            _context.Entry(provider).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Providers.AnyAsync(e => e.ProviderID == id)) return false;
                throw;
            }
        }

        public async Task<bool> DeleteProviderAsync(Guid id)
        {
            var entity = await _context.Providers.FindAsync(id);
            if (entity == null) return false;
            _context.Providers.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ── Appointment ───────────────────────────────────────────────────────

        public async Task<IEnumerable<Appointment>> GetAppointmentsAsync()
            => await _context.Appointments.ToListAsync();

        public async Task<Appointment?> GetAppointmentAsync(Guid id)
            => await _context.Appointments.FindAsync(id);

        public async Task<Appointment> AddAppointmentAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<bool> UpdateAppointmentAsync(Guid id, Appointment appointment)
        {
            if (id != appointment.AppID) return false;
            _context.Entry(appointment).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Appointments.AnyAsync(e => e.AppID == id)) return false;
                throw;
            }
        }

        public async Task<bool> DeleteAppointmentAsync(Guid id)
        {
            var entity = await _context.Appointments.FindAsync(id);
            if (entity == null) return false;
            _context.Appointments.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ── Waitlist ──────────────────────────────────────────────────────────

        public async Task<IEnumerable<Waitlist>> GetWaitlistsAsync()
            => await _context.Waitlists.ToListAsync();

        public async Task<Waitlist?> GetWaitlistAsync(Guid id)
            => await _context.Waitlists.FindAsync(id);

        public async Task<Waitlist> AddWaitlistAsync(Waitlist waitlist)
        {
            _context.Waitlists.Add(waitlist);
            await _context.SaveChangesAsync();
            return waitlist;
        }

        public async Task<bool> UpdateWaitlistAsync(Guid id, Waitlist waitlist)
        {
            if (id != waitlist.WaitID) return false;
            _context.Entry(waitlist).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Waitlists.AnyAsync(e => e.WaitID == id)) return false;
                throw;
            }
        }

        public async Task<bool> DeleteWaitlistAsync(Guid id)
        {
            var entity = await _context.Waitlists.FindAsync(id);
            if (entity == null) return false;
            _context.Waitlists.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        // ── Room ──────────────────────────────────────────────────────────────

        public async Task<IEnumerable<Room>> GetRoomsAsync()
            => await _context.Rooms.ToListAsync();

        public async Task<Room?> GetRoomAsync(Guid id)
            => await _context.Rooms.FindAsync(id);

        public async Task<Room> AddRoomAsync(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<bool> UpdateRoomAsync(Guid id, Room room)
        {
            if (id != room.RoomID) return false;
            _context.Entry(room).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Rooms.AnyAsync(e => e.RoomID == id)) return false;
                throw;
            }
        }

        public async Task<bool> DeleteRoomAsync(Guid id)
        {
            var entity = await _context.Rooms.FindAsync(id);
            if (entity == null) return false;
            _context.Rooms.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}