using ClinicFlow_Backend.DTO;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow_Backend.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class SchedulingController : ControllerBase
    {
        private readonly ISchedulingRepository _repository;

        private static readonly string[] AllowedProviderStatuses =
            { "Active", "OnLeave", "Inactive" };
        private static readonly string[] AllowedAppointmentModes =
            { "InPerson", "Telehealth" };
        private static readonly string[] AllowedAppointmentStatuses =
            { "Scheduled", "Confirmed", "CheckedIn", "Completed", "Cancelled", "NoShow" };
        private static readonly string[] AllowedWaitlistStatuses =
            { "Active", "Offered", "Booked", "Expired", "Cancelled" };
        private static readonly string[] AllowedRoomStatuses =
            { "Available", "Occupied", "Maintenance" };

        public SchedulingController(ISchedulingRepository repository)
        {
            _repository = repository;
        }

        // ── Provider ──────────────────────────────────────────────────────────

        [HttpGet("providers")]
        public async Task<ActionResult<IEnumerable<ProviderDto>>> GetProviders()
        {
            try
            {
                var providers = await _repository.GetProvidersAsync();
                return Ok(providers.Select(p => MapProvider(p)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("providers/{id}")]
        public async Task<ActionResult<ProviderDto>> GetProvider(Guid id)
        {
            try
            {
                var provider = await _repository.GetProviderAsync(id);
                if (provider == null)
                    return NotFound(new { message = $"Provider with ID {id} was not found." });

                return Ok(MapProvider(provider));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpPost("providers")]
        public async Task<ActionResult<ProviderDto>> PostProvider(CreateProviderDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Specialty))
                return BadRequest(new { message = "Specialty is required." });

            try
            {
                var provider = new Provider
                {
                    UserID = dto.UserID,
                    Specialty = dto.Specialty,
                    ClinicIDsJSON = dto.ClinicIDsJSON,
                    AvailabilityJSON = dto.AvailabilityJSON,
                    MaxDailySlots = dto.MaxDailySlots,
                    Status = "Active"
                };
                await _repository.AddProviderAsync(provider);
                return CreatedAtAction(nameof(GetProvider), new { id = provider.ProviderID }, MapProvider(provider));
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "Failed to create provider." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpPut("providers/{id}")]
        public async Task<IActionResult> PutProvider(Guid id, UpdateProviderDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Specialty))
                return BadRequest(new { message = "Specialty is required." });

            if (string.IsNullOrWhiteSpace(dto.Status) || !AllowedProviderStatuses.Contains(dto.Status))
                return BadRequest(new { message = $"Status must be one of: {string.Join(", ", AllowedProviderStatuses)}." });

            try
            {
                var existing = await _repository.GetProviderAsync(id);
                if (existing == null)
                    return NotFound(new { message = $"Provider with ID {id} was not found." });

                existing.Specialty = dto.Specialty;
                existing.ClinicIDsJSON = dto.ClinicIDsJSON;
                existing.AvailabilityJSON = dto.AvailabilityJSON;
                existing.MaxDailySlots = dto.MaxDailySlots;
                existing.Status = dto.Status;

                await _repository.UpdateProviderAsync(id, existing);
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "Update failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpDelete("providers/{id}")]
        public async Task<IActionResult> DeleteProvider(Guid id)
        {
            try
            {
                var result = await _repository.DeleteProviderAsync(id);
                if (!result)
                    return NotFound(new { message = $"Provider with ID {id} was not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        // ── Appointment ───────────────────────────────────────────────────────

        [HttpGet("appointments")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments()
        {
            try
            {
                var appointments = await _repository.GetAppointmentsAsync();
                return Ok(appointments.Select(a => MapAppointment(a)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("appointments/{id}")]
        public async Task<ActionResult<AppointmentDto>> GetAppointment(Guid id)
        {
            try
            {
                var appointment = await _repository.GetAppointmentAsync(id);
                if (appointment == null)
                    return NotFound(new { message = $"Appointment with ID {id} was not found." });

                return Ok(MapAppointment(appointment));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpPost("appointments")]
        public async Task<ActionResult<AppointmentDto>> PostAppointment(CreateAppointmentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Mode) || !AllowedAppointmentModes.Contains(dto.Mode))
                return BadRequest(new { message = $"Mode must be one of: {string.Join(", ", AllowedAppointmentModes)}." });

            if (string.IsNullOrWhiteSpace(dto.Reason))
                return BadRequest(new { message = "Reason is required." });

            try
            {
                var appointment = new Appointment
                {
                    PatientID = dto.PatientID,
                    ProviderID = dto.ProviderID,
                    ClinicID = dto.ClinicID,
                    ScheduledAt = dto.ScheduledAt,
                    DurationMinutes = dto.DurationMinutes,
                    Mode = dto.Mode,
                    Reason = dto.Reason,
                    CreatedBy = dto.CreatedBy,
                    Status = "Scheduled",
                    CreatedAt = DateTime.UtcNow
                };
                await _repository.AddAppointmentAsync(appointment);
                return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppID }, MapAppointment(appointment));
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "Failed to book appointment." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpPut("appointments/{id}")]
        public async Task<IActionResult> PutAppointment(Guid id, UpdateAppointmentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Mode) || !AllowedAppointmentModes.Contains(dto.Mode))
                return BadRequest(new { message = $"Mode must be one of: {string.Join(", ", AllowedAppointmentModes)}." });

            if (string.IsNullOrWhiteSpace(dto.Status) || !AllowedAppointmentStatuses.Contains(dto.Status))
                return BadRequest(new { message = $"Status must be one of: {string.Join(", ", AllowedAppointmentStatuses)}." });

            try
            {
                var existing = await _repository.GetAppointmentAsync(id);
                if (existing == null)
                    return NotFound(new { message = $"Appointment with ID {id} was not found." });

                existing.ScheduledAt = dto.ScheduledAt;
                existing.DurationMinutes = dto.DurationMinutes;
                existing.Mode = dto.Mode;
                existing.Reason = dto.Reason;
                existing.Status = dto.Status;

                await _repository.UpdateAppointmentAsync(id, existing);
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "Update failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpDelete("appointments/{id}")]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            try
            {
                var result = await _repository.DeleteAppointmentAsync(id);
                if (!result)
                    return NotFound(new { message = $"Appointment with ID {id} was not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        // ── Waitlist ──────────────────────────────────────────────────────────

        [HttpGet("waitlist")]
        public async Task<ActionResult<IEnumerable<WaitlistDto>>> GetWaitlists()
        {
            try
            {
                var waitlists = await _repository.GetWaitlistsAsync();
                return Ok(waitlists.Select(w => MapWaitlist(w)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("waitlist/{id}")]
        public async Task<ActionResult<WaitlistDto>> GetWaitlist(Guid id)
        {
            try
            {
                var waitlist = await _repository.GetWaitlistAsync(id);
                if (waitlist == null)
                    return NotFound(new { message = $"Waitlist entry with ID {id} was not found." });

                return Ok(MapWaitlist(waitlist));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpPost("waitlist")]
        public async Task<ActionResult<WaitlistDto>> PostWaitlist(CreateWaitlistDto dto)
        {
            if (dto.RequestedWindowStart == default || dto.RequestedWindowEnd == default)
                return BadRequest(new { message = "RequestedWindowStart and RequestedWindowEnd are required." });

            if (dto.RequestedWindowEnd <= dto.RequestedWindowStart)
                return BadRequest(new { message = "RequestedWindowEnd must be after RequestedWindowStart." });

            try
            {
                var waitlist = new Waitlist
                {
                    PatientID = dto.PatientID,
                    ProviderID = dto.ProviderID,
                    RequestedWindowStart = dto.RequestedWindowStart,
                    RequestedWindowEnd = dto.RequestedWindowEnd,
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow
                };
                await _repository.AddWaitlistAsync(waitlist);
                return CreatedAtAction(nameof(GetWaitlist), new { id = waitlist.WaitID }, MapWaitlist(waitlist));
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "Failed to add to waitlist." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpPut("waitlist/{id}")]
        public async Task<IActionResult> PutWaitlist(Guid id, UpdateWaitlistDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Status) || !AllowedWaitlistStatuses.Contains(dto.Status))
                return BadRequest(new { message = $"Status must be one of: {string.Join(", ", AllowedWaitlistStatuses)}." });

            try
            {
                var existing = await _repository.GetWaitlistAsync(id);
                if (existing == null)
                    return NotFound(new { message = $"Waitlist entry with ID {id} was not found." });

                existing.RequestedWindowStart = dto.RequestedWindowStart;
                existing.RequestedWindowEnd = dto.RequestedWindowEnd;
                existing.Status = dto.Status;

                await _repository.UpdateWaitlistAsync(id, existing);
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "Update failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpDelete("waitlist/{id}")]
        public async Task<IActionResult> DeleteWaitlist(Guid id)
        {
            try
            {
                var result = await _repository.DeleteWaitlistAsync(id);
                if (!result)
                    return NotFound(new { message = $"Waitlist entry with ID {id} was not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        // ── Room ──────────────────────────────────────────────────────────────

        [HttpGet("rooms")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetRooms()
        {
            try
            {
                var rooms = await _repository.GetRoomsAsync();
                return Ok(rooms.Select(r => MapRoom(r)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpGet("rooms/{id}")]
        public async Task<ActionResult<RoomDto>> GetRoom(Guid id)
        {
            try
            {
                var room = await _repository.GetRoomAsync(id);
                if (room == null)
                    return NotFound(new { message = $"Room with ID {id} was not found." });

                return Ok(MapRoom(room));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpPost("rooms")]
        public async Task<ActionResult<RoomDto>> PostRoom(CreateRoomDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Name is required." });

            try
            {
                var room = new Room
                {
                    ClinicID = dto.ClinicID,
                    Name = dto.Name,
                    Capacity = dto.Capacity,
                    ResourcesJSON = dto.ResourcesJSON,
                    Status = "Available"
                };
                await _repository.AddRoomAsync(room);
                return CreatedAtAction(nameof(GetRoom), new { id = room.RoomID }, MapRoom(room));
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "Failed to create room." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpPut("rooms/{id}")]
        public async Task<IActionResult> PutRoom(Guid id, UpdateRoomDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Name is required." });

            if (string.IsNullOrWhiteSpace(dto.Status) || !AllowedRoomStatuses.Contains(dto.Status))
                return BadRequest(new { message = $"Status must be one of: {string.Join(", ", AllowedRoomStatuses)}." });

            try
            {
                var existing = await _repository.GetRoomAsync(id);
                if (existing == null)
                    return NotFound(new { message = $"Room with ID {id} was not found." });

                existing.Name = dto.Name;
                existing.Capacity = dto.Capacity;
                existing.ResourcesJSON = dto.ResourcesJSON;
                existing.Status = dto.Status;

                await _repository.UpdateRoomAsync(id, existing);
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "Update failed." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        [HttpDelete("rooms/{id}")]
        public async Task<IActionResult> DeleteRoom(Guid id)
        {
            try
            {
                var result = await _repository.DeleteRoomAsync(id);
                if (!result)
                    return NotFound(new { message = $"Room with ID {id} was not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", detail = ex.Message });
            }
        }

        // ── Mappers ───────────────────────────────────────────────────────────

        private static ProviderDto MapProvider(Provider p) => new ProviderDto
        {
            ProviderID = p.ProviderID,
            UserID = p.UserID,
            Specialty = p.Specialty,
            ClinicIDsJSON = p.ClinicIDsJSON,
            AvailabilityJSON = p.AvailabilityJSON,
            MaxDailySlots = p.MaxDailySlots,
            Status = p.Status
        };

        private static AppointmentDto MapAppointment(Appointment a) => new AppointmentDto
        {
            AppID = a.AppID,
            PatientID = a.PatientID,
            ProviderID = a.ProviderID,
            ClinicID = a.ClinicID,
            ScheduledAt = a.ScheduledAt,
            DurationMinutes = a.DurationMinutes,
            Mode = a.Mode,
            Reason = a.Reason,
            Status = a.Status,
            CreatedBy = a.CreatedBy,
            CreatedAt = a.CreatedAt
        };

        private static WaitlistDto MapWaitlist(Waitlist w) => new WaitlistDto
        {
            WaitID = w.WaitID,
            PatientID = w.PatientID,
            ProviderID = w.ProviderID,
            RequestedWindowStart = w.RequestedWindowStart,
            RequestedWindowEnd = w.RequestedWindowEnd,
            CreatedAt = w.CreatedAt,
            Status = w.Status
        };

        private static RoomDto MapRoom(Room r) => new RoomDto
        {
            RoomID = r.RoomID,
            ClinicID = r.ClinicID,
            Name = r.Name,
            Capacity = r.Capacity,
            ResourcesJSON = r.ResourcesJSON,
            Status = r.Status
        };
    }
}