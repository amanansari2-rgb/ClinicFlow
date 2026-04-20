using ClinicFlow_Backend.Model;

namespace ClinicFlow_Backend.Repositories.Interface
{
    public interface ISchedulingRepository
    {
        // Provider
        Task<IEnumerable<Provider>> GetProvidersAsync();
        Task<Provider?> GetProviderAsync(Guid id);
        Task<Provider> AddProviderAsync(Provider provider);
        Task<bool> UpdateProviderAsync(Guid id, Provider provider);
        Task<bool> DeleteProviderAsync(Guid id);

        // Appointment
        Task<IEnumerable<Appointment>> GetAppointmentsAsync();
        Task<Appointment?> GetAppointmentAsync(Guid id);
        Task<Appointment> AddAppointmentAsync(Appointment appointment);
        Task<bool> UpdateAppointmentAsync(Guid id, Appointment appointment);
        Task<bool> DeleteAppointmentAsync(Guid id);

        // Waitlist
        Task<IEnumerable<Waitlist>> GetWaitlistsAsync();
        Task<Waitlist?> GetWaitlistAsync(Guid id);
        Task<Waitlist> AddWaitlistAsync(Waitlist waitlist);
        Task<bool> UpdateWaitlistAsync(Guid id, Waitlist waitlist);
        Task<bool> DeleteWaitlistAsync(Guid id);

        // Room
        Task<IEnumerable<Room>> GetRoomsAsync();
        Task<Room?> GetRoomAsync(Guid id);
        Task<Room> AddRoomAsync(Room room);
        Task<bool> UpdateRoomAsync(Guid id, Room room);
        Task<bool> DeleteRoomAsync(Guid id);
    }
}