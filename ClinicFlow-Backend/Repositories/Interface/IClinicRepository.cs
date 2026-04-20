using ClinicFlow_Backend.Model;

namespace ClinicFlow_Backend.Repositories.Interface
{
    public interface IClinicRepository
    {
        Task<IEnumerable<Clinic>> GetClinicsAsync();
        Task<Clinic?> GetClinicAsync(Guid id);
        Task<Clinic> PostClinicAsync(Clinic clinic);
        Task<bool> PutClinicAsync(Guid id, Clinic clinic);
        Task<bool> DeleteClinicAsync(Guid id);
    }
}
