using ClinicFlow_Backend.DTO;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow_Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    //[Authorize(Roles = "Admin")] // only Admin configures clinics
    public class ClinicsController : ControllerBase
    {
        private readonly IClinicRepository _repository;

        private static readonly string[] AllowedClinicStatuses = { "Active", "Inactive" };

        public ClinicsController(IClinicRepository repository)
        {
            _repository = repository;
        }

        // GET: /Clinics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClinicDto>>> GetClinics()
        {
            try
            {
                var clinics = await _repository.GetClinicsAsync();
                return Ok(clinics.Select(c => MapToDto(c)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving clinics.", detail = ex.Message });
            }
        }

        // GET: /Clinics/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ClinicDto>> GetClinic(Guid id)
        {

            try
            {
                var clinic = await _repository.GetClinicAsync(id);

                if (clinic == null)
                    return NotFound(new { message = $"Clinic with ID {id} not found." });

                return Ok(MapToDto(clinic));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the clinic.", detail = ex.Message });
            }
        }

        // POST: /Clinics
        [HttpPost]
        public async Task<ActionResult<ClinicDto>> PostClinic(CreateClinicDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var clinic = new Clinic
                {
                    Name = dto.Name,
                    AddressJSON = dto.AddressJSON,
                    Status = "Active"
                };

                await _repository.PostClinicAsync(clinic);

                return CreatedAtAction(nameof(GetClinic), new { id = clinic.ClinicID }, MapToDto(clinic));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while creating the clinic.", detail = ex.Message });
            }
        }

        // PUT: /Clinics/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClinic(Guid id, UpdateClinicDto dto)
        {

            if (string.IsNullOrWhiteSpace(dto.Status) || !AllowedClinicStatuses.Contains(dto.Status))
                return BadRequest(new { message = $"Status must be one of: {string.Join(", ", AllowedClinicStatuses)}." });

            try
            {
                var existing = await _repository.GetClinicAsync(id);
                if (existing == null)
                    return NotFound(new { message = $"Clinic with ID {id} not found." });

                existing.Name = dto.Name;
                existing.AddressJSON = dto.AddressJSON;
                existing.Status = dto.Status;

                var result = await _repository.PutClinicAsync(id, existing);

                if (!result)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to update clinic." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the clinic.", detail = ex.Message });
            }
        }

        // DELETE: /Clinics/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClinic(Guid id)
        {

            try
            {
                var result = await _repository.DeleteClinicAsync(id);

                if (!result)
                    return NotFound(new { message = $"Clinic with ID {id} not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the clinic.", detail = ex.Message });
            }
        }

        private static ClinicDto MapToDto(Clinic clinic) => new ClinicDto
        {
            ClinicID = clinic.ClinicID,
            Name = clinic.Name,
            AddressJSON = clinic.AddressJSON,
            Status = clinic.Status
        };
    }
}
