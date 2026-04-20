using ClinicFlow_Backend.DTO;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow_Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClinicsController : ControllerBase
    {
        private readonly IClinicRepository _repository;

        public ClinicsController(IClinicRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Clinics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClinicDto>>> GetClinics()
        {
            try
            {
                var clinics = await _repository.GetClinicsAsync();
                return Ok(clinics.Select(c => MapToDto(c)));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving clinics.");
            }
        }

        // GET: api/Clinics/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ClinicDto>> GetClinic(Guid id)
        {
            try
            {
                var clinic = await _repository.GetClinicAsync(id);

                if (clinic == null)
                    return NotFound(new { message = $"Clinic with ID {id} was not found." });

                return Ok(MapToDto(clinic));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while retrieving the clinic." });
            }
        }

        // POST: api/Clinics
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the clinic.");
            }
        }

        // PUT: api/Clinics/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClinic(Guid id, UpdateClinicDto dto)
        {
            try
            {
                var existing = await _repository.GetClinicAsync(id);
                if (existing == null)
                    return NotFound(new { message = $"Clinic with ID {id} was not found." });

                existing.Name = dto.Name;
                existing.AddressJSON = dto.AddressJSON;
                existing.Status = dto.Status;

                var result = await _repository.PutClinicAsync(id, existing);

                if (!result)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Failed to update clinic." });

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while updating the clinic." });
            }
        }

        // DELETE: api/Clinics/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClinic(Guid id)
        {
            try
            {
                var result = await _repository.DeleteClinicAsync(id);

                if (!result)
                    return NotFound(new { message = $"Clinic with ID {id} was not found." });

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while deleting the clinic." });
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
