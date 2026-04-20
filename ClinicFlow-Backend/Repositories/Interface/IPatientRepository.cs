using ClinicFlow_Backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow_Backend.Repositories.Interface
{
    public interface IPatientRepository
    {
        //Paitent Models
        Task<IEnumerable<Patient>> GetPatientsAsync();
        Task<Patient?> GetPatientAsync(Guid id);
        Task<bool> PutPatientAsync(Guid id, Patient patient);
        Task<Patient> PostPatientAsync(Patient patient);
        Task<bool> DeletePatientAsync(Guid id);


        //Intake Models
        Task<IEnumerable<IntakeForm>> GetIntakeFormsByPatientAsync(Guid patientId);
        Task<IntakeForm?> GetIntakeFormAsync(Guid id);
        Task<IntakeForm> AddIntakeFormAsync(IntakeForm form);
        Task<bool> UpdateIntakeFormAsync(Guid id, IntakeForm form);
        Task<bool> DeleteIntakeFormAsync(Guid id);
    }
}
