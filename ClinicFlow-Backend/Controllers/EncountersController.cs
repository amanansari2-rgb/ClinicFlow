using ClinicFlow_Backend.DTO;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow_Backend.Controllers
{
    [Route("api/v1/encounters")]
    [ApiController]
    public class EncountersController : ControllerBase
    {
        private readonly IEncounterRepository _repository;

        public EncountersController(IEncounterRepository repository)
        {
            _repository = repository;
        }

        // ══════════════════════════════════════════════════════════════════
        //  ENCOUNTER CRUD
        // ══════════════════════════════════════════════════════════════════

        // GET: api/v1/encounters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EncounterDto>>> GetEncounters()
        {
            try
            {
                var encounters = await _repository.GetEncountersAsync();
                return Ok(encounters.Select(e => MapToDto(e)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to retrieve encounters.", detail = ex.Message });
            }
        }

        // GET: api/v1/encounters/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EncounterDto>> GetEncounter(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { error = "A valid Encounter ID is required." });

            try
            {
                var encounter = await _repository.GetEncounterAsync(id);
                if (encounter == null)
                    return NotFound(new { error = $"Encounter with ID '{id}' not found." });

                return Ok(MapToDto(encounter));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to retrieve the encounter.", detail = ex.Message });
            }
        }

        // POST: api/v1/encounters
        [HttpPost]
        public async Task<ActionResult<EncounterDto>> PostEncounter(CreateEncounterDto dto)
        {
            try
            {
                var encounter = new Encounter
                {
                    AppID = dto.AppID,
                    PatientID = dto.PatientID,
                    ProviderID = dto.ProviderID,
                    StartAt = dto.StartAt,
                    VisitType = dto.VisitType.Trim(),
                    ChiefComplaint = dto.ChiefComplaint.Trim(),
                    Status = "InProgress"
                };

                await _repository.PostEncounterAsync(encounter);
                return CreatedAtAction(nameof(GetEncounter), new { id = encounter.EncounterID }, MapToDto(encounter));
            }
            catch (DbUpdateException ex)
            {
                return Conflict(new { error = "Could not create encounter. Check that AppID, PatientID, and ProviderID are valid.", detail = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to create encounter.", detail = ex.Message });
            }
        }

        // PUT: api/v1/encounters/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEncounter(Guid id, UpdateEncounterDto dto)
        {
            if (id == Guid.Empty)
                return BadRequest(new { error = "A valid Encounter ID is required." });

            try
            {
                var existing = await _repository.GetEncounterAsync(id);
                if (existing == null)
                    return NotFound(new { error = $"Encounter with ID '{id}' not found." });

                existing.EndAt = dto.EndAt;
                existing.VisitType = dto.VisitType.Trim();
                existing.ChiefComplaint = dto.ChiefComplaint.Trim();
                existing.EncounterNoteURI = dto.EncounterNoteURI?.Trim();
                existing.Status = dto.Status.Trim();

                var result = await _repository.PutEncounterAsync(id, existing);
                if (!result)
                    return StatusCode(500, new { error = "Failed to update the encounter." });

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return Conflict(new { error = "Could not update encounter. Possible concurrency conflict.", detail = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to update the encounter.", detail = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════════════════
        //  ORDERS (nested under Encounter)
        // ══════════════════════════════════════════════════════════════════

        // GET: api/v1/encounters/{id}/orders
        [HttpGet("{id}/orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { error = "A valid Encounter ID is required." });

            try
            {
                var encounter = await _repository.GetEncounterAsync(id);
                if (encounter == null)
                    return NotFound(new { error = $"Encounter with ID '{id}' not found." });

                var orders = await _repository.GetOrdersByEncounterAsync(id);
                return Ok(orders.Select(o => MapOrderToDto(o)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to retrieve orders.", detail = ex.Message });
            }
        }

        // POST: api/v1/encounters/{id}/orders
        [HttpPost("{id}/orders")]
        public async Task<ActionResult<OrderDto>> PostOrder(Guid id, CreateOrderDto dto)
        {
            if (id == Guid.Empty)
                return BadRequest(new { error = "A valid Encounter ID is required." });

            try
            {
                var encounter = await _repository.GetEncounterAsync(id);
                if (encounter == null)
                    return NotFound(new { error = $"Encounter with ID '{id}' not found." });

                var order = new Order
                {
                    EncounterID = id,
                    OrderedBy = dto.OrderedBy,
                    OrderType = dto.OrderType.Trim(),
                    OrderDetailsJSON = dto.OrderDetailsJSON.Trim(),
                    ExpectedAt = dto.ExpectedAt,
                    Status = "Ordered"
                };

                await _repository.PostOrderAsync(order);
                return CreatedAtAction(nameof(GetOrders), new { id }, MapOrderToDto(order));
            }
            catch (DbUpdateException ex)
            {
                return Conflict(new { error = "Could not create order. Check that OrderedBy (User ID) is valid.", detail = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to create order.", detail = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════════════════
        //  REFERRALS (nested under Encounter)
        // ══════════════════════════════════════════════════════════════════

        // GET: api/v1/encounters/{id}/referrals
        [HttpGet("{id}/referrals")]
        public async Task<ActionResult<IEnumerable<ReferralDto>>> GetReferrals(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { error = "A valid Encounter ID is required." });

            try
            {
                var encounter = await _repository.GetEncounterAsync(id);
                if (encounter == null)
                    return NotFound(new { error = $"Encounter with ID '{id}' not found." });

                var referrals = await _repository.GetReferralsByEncounterAsync(id);
                return Ok(referrals.Select(r => MapReferralToDto(r)));
            }
            catch (Exception ex)

            {
                return StatusCode(500, new { error = "Failed to retrieve referrals.", detail = ex.Message });
            }
        }

        // POST: api/v1/encounters/{id}/referrals
        [HttpPost("{id}/referrals")]
        public async Task<ActionResult<ReferralDto>> PostReferral(Guid id, CreateReferralDto dto)
        {
            if (id == Guid.Empty)
                return BadRequest(new { error = "A valid Encounter ID is required." });

            try
            {
                var encounter = await _repository.GetEncounterAsync(id);
                if (encounter == null)
                    return NotFound(new { error = $"Encounter with ID '{id}' not found." });

                var referral = new Referral
                {
                    EncounterID = id,
                    FromProvider = dto.FromProvider,
                    ToProviderInfoJSON = dto.ToProviderInfoJSON.Trim(),
                    AppointmentID = dto.AppointmentID,
                    Status = "Sent"
                };

                await _repository.PostReferralAsync(referral);
                return CreatedAtAction(nameof(GetReferrals), new { id }, MapReferralToDto(referral));
            }
            catch (DbUpdateException ex)
            {
                return Conflict(new { error = "Could not create referral. Check that FromProvider and AppointmentID are valid.", detail = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to create referral.", detail = ex.Message });
            }
        }

        // ══════════════════════════════════════════════════════════════════
        //  MAPPING HELPERS
        // ══════════════════════════════════════════════════════════════════

        private static EncounterDto MapToDto(Encounter e) => new EncounterDto
        {
            EncounterID = e.EncounterID,
            AppID = e.AppID,
            PatientID = e.PatientID,
            ProviderID = e.ProviderID,
            StartAt = e.StartAt,
            EndAt = e.EndAt,
            VisitType = e.VisitType,
            ChiefComplaint = e.ChiefComplaint,
            EncounterNoteURI = e.EncounterNoteURI,
            Status = e.Status
        };

        private static OrderDto MapOrderToDto(Order o) => new OrderDto
        {
            OrderID = o.OrderID,
            EncounterID = o.EncounterID,
            OrderedBy = o.OrderedBy,
            OrderType = o.OrderType,
            OrderDetailsJSON = o.OrderDetailsJSON,
            OrderedAt = o.OrderedAt,
            ExpectedAt = o.ExpectedAt,
            Status = o.Status
        };

        private static ReferralDto MapReferralToDto(Referral r) => new ReferralDto
        {
            ReferralID = r.ReferralID,
            EncounterID = r.EncounterID,
            FromProvider = r.FromProvider,
            ToProviderInfoJSON = r.ToProviderInfoJSON,
            SentAt = r.SentAt,
            AcceptedAt = r.AcceptedAt,
            AppointmentID = r.AppointmentID,
            Status = r.Status
        };
    }
}
