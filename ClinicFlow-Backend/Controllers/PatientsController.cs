using ClinicFlow_Backend.DTO;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow_Backend.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientRepository _repository;

        private static readonly string[] AllowedGenders =
            { "Male", "Female", "Other", "Prefer Not To Say" };

        private static readonly string[] AllowedConsentStatuses =
            { "Pending", "Signed", "Revoked" };

        private static readonly string[] AllowedIntakeSources =
            { "Portal", "Kiosk", "Staff" };

        public PatientsController(IPatientRepository repository)
        {
            _repository = repository;
        }

        // ── Patient CRUD ──────────────────────────────────────────────────────

        // GET: api/v1/patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients()
        {
            try
            {
                var patients = await _repository.GetPatientsAsync();
                return Ok(patients.Select(p => MapToDto(p)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        // GET: api/v1/patients/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> GetPatient(Guid id)
        {
            try
            {
                var patient = await _repository.GetPatientAsync(id);
                if (patient == null)
                    return NotFound(new { message = $"Patient with ID {id} was not found." });

                return Ok(MapToDto(patient));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        // POST: api/v1/patients
        [HttpPost]
        public async Task<ActionResult<PatientDto>> PostPatient(CreatePatientDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MRN))
                return BadRequest(new { message = "MRN is required." });

            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Name is required." });

            if (string.IsNullOrWhiteSpace(dto.Gender) || !AllowedGenders.Contains(dto.Gender))
                return BadRequest(new { message = $"Gender must be one of: {string.Join(", ", AllowedGenders)}." });

            try
            {
                var patient = new Patient
                {
                    UserID = dto.UserID,
                    MRN = dto.MRN,
                    Name = dto.Name,
                    DOB = dto.DOB,
                    Gender = dto.Gender,
                    ContactInfoJSON = dto.ContactInfoJSON,
                    AddressJSON = dto.AddressJSON,
                    InsuranceInfoJSON = dto.InsuranceInfoJSON,
                    ConsentStatus = "Pending",
                    CreatedAt = DateTime.UtcNow
                };

                await _repository.PostPatientAsync(patient);
                return CreatedAtAction(nameof(GetPatient), new { id = patient.PatientID }, MapToDto(patient));
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "A patient with this MRN already exists." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        // PUT: api/v1/patients/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(Guid id, UpdatePatientDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Name is required." });

            if (string.IsNullOrWhiteSpace(dto.Gender) || !AllowedGenders.Contains(dto.Gender))
                return BadRequest(new { message = $"Gender must be one of: {string.Join(", ", AllowedGenders)}." });

            if (string.IsNullOrWhiteSpace(dto.ConsentStatus) || !AllowedConsentStatuses.Contains(dto.ConsentStatus))
                return BadRequest(new { message = $"ConsentStatus must be one of: {string.Join(", ", AllowedConsentStatuses)}." });

            try
            {
                var existing = await _repository.GetPatientAsync(id);
                if (existing == null)
                    return NotFound(new { message = $"Patient with ID {id} was not found." });

                existing.Name = dto.Name;
                existing.DOB = dto.DOB;
                existing.Gender = dto.Gender;
                existing.ContactInfoJSON = dto.ContactInfoJSON;
                existing.AddressJSON = dto.AddressJSON;
                existing.InsuranceInfoJSON = dto.InsuranceInfoJSON;
                existing.ConsentStatus = dto.ConsentStatus;

                await _repository.PutPatientAsync(id, existing);
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "Update failed — data conflict." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        // DELETE: api/v1/patients/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            try
            {
                var result = await _repository.DeletePatientAsync(id);
                if (!result)
                    return NotFound(new { message = $"Patient with ID {id} was not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        // ── IntakeForm (nested under patient) ─────────────────────────────────

        // GET: api/v1/patients/{patientId}/intake-forms
        [HttpGet("{patientId}/intake-forms")]
        public async Task<ActionResult<IEnumerable<IntakeFormDto>>> GetIntakeForms(Guid patientId)
        {
            try
            {
                var patient = await _repository.GetPatientAsync(patientId);
                if (patient == null)
                    return NotFound(new { message = $"Patient with ID {patientId} was not found." });

                var forms = await _repository.GetIntakeFormsByPatientAsync(patientId);
                return Ok(forms.Select(f => MapToIntakeFormDto(f)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        // POST: api/v1/patients/{patientId}/intake-forms
        [HttpPost("{patientId}/intake-forms")]
        public async Task<ActionResult<IntakeFormDto>> PostIntakeForm(Guid patientId, CreateIntakeFormDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.VisitReason))
                return BadRequest(new { message = "VisitReason is required." });

            if (string.IsNullOrWhiteSpace(dto.Source) || !AllowedIntakeSources.Contains(dto.Source))
                return BadRequest(new { message = $"Source must be one of: {string.Join(", ", AllowedIntakeSources)}." });

            try
            {
                var patient = await _repository.GetPatientAsync(patientId);
                if (patient == null)
                    return NotFound(new { message = $"Patient with ID {patientId} was not found." });

                var form = new IntakeForm
                {
                    PatientID = patientId,
                    VisitReason = dto.VisitReason,
                    SymptomsJSON = dto.SymptomsJSON,
                    AllergiesJSON = dto.AllergiesJSON,
                    MedicationsJSON = dto.MedicationsJSON,
                    Source = dto.Source,
                    Status = "Draft"
                };

                await _repository.AddIntakeFormAsync(form);
                return CreatedAtAction(nameof(GetIntakeForms), new { patientId = patientId }, MapToIntakeFormDto(form));
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "Failed to save intake form — data conflict." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        // ── Mappers ───────────────────────────────────────────────────────────

        private static PatientDto MapToDto(Patient p) => new PatientDto
        {
            PatientID = p.PatientID,
            UserID = p.UserID,
            MRN = p.MRN,
            Name = p.Name,
            DOB = p.DOB,
            Gender = p.Gender,
            ContactInfoJSON = p.ContactInfoJSON,
            AddressJSON = p.AddressJSON,
            InsuranceInfoJSON = p.InsuranceInfoJSON,
            ConsentStatus = p.ConsentStatus,
            CreatedAt = p.CreatedAt
        };

        private static IntakeFormDto MapToIntakeFormDto(IntakeForm f) => new IntakeFormDto
        {
            FormID = f.FormID,
            PatientID = f.PatientID,
            VisitReason = f.VisitReason,
            SymptomsJSON = f.SymptomsJSON,
            AllergiesJSON = f.AllergiesJSON,
            MedicationsJSON = f.MedicationsJSON,
            CompletedAt = f.CompletedAt,
            Source = f.Source,
            Status = f.Status
        };
    }
}