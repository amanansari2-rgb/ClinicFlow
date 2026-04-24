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
    //[Authorize(Roles = "Admin,Billing")] // only Billing Officers and Admins
    public class BillingController : ControllerBase
    {
        private readonly IBillingRepository _repo;

        public BillingController(IBillingRepository repo)
        {
            _repo = repo;
        }

        // ─────────────────────────────────────────────────────────
        // CHARGES
        // ─────────────────────────────────────────────────────────

        // GET api/v1/billing/charges
        [HttpGet("charges")]
        public async Task<ActionResult<IEnumerable<ChargeDto>>> GetCharges(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                if (page < 1) return BadRequest("Page must be greater than 0.");
                if (pageSize < 1) return BadRequest("Page size must be greater than 0.");

                var charges = await _repo.GetChargesAsync(page, pageSize);
                return Ok(charges.Select(MapCharge));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve charges: {ex.Message}");
            }
        }

        // POST api/v1/billing/charges
        [HttpPost("charges")]
        public async Task<ActionResult<ChargeDto>> CreateCharge([FromBody] CreateChargeDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Request body cannot be null.");

                var charge = new Charge
                {
                    EncounterID  = dto.EncounterID,
                    CPTCodesJSON = dto.CPTCodesJSON,
                    ICDCodesJSON = dto.ICDCodesJSON,
                    Units        = dto.Units,
                    UnitPrice    = dto.UnitPrice,
                    Status       = "Draft"
                };

                var created = await _repo.CreateChargeAsync(charge);
                return CreatedAtAction(nameof(GetCharges), new { id = created.ChargeID }, MapCharge(created));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create charge: {ex.Message}");
            }
        }

        // PUT api/v1/billing/charges/{id}
        [HttpPut("charges/{id:guid}")]
        public async Task<ActionResult<ChargeDto>> UpdateCharge(Guid id, [FromBody] UpdateChargeDto dto)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Charge ID cannot be empty.");

                if (dto == null)
                    return BadRequest("Request body cannot be null.");

                var patch = new Charge
                {
                    CPTCodesJSON = dto.CPTCodesJSON!,
                    ICDCodesJSON = dto.ICDCodesJSON!,
                    Units        = dto.Units ?? 0,
                    UnitPrice    = dto.UnitPrice ?? 0,
                    Status       = dto.Status!
                };

                var updated = await _repo.UpdateChargeAsync(id, patch);
                if (updated is null)
                    return NotFound($"Charge with ID '{id}' was not found.");

                return Ok(MapCharge(updated));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update charge: {ex.Message}");
            }
        }

        // ─────────────────────────────────────────────────────────
        // INVOICES
        // ─────────────────────────────────────────────────────────

        // GET api/v1/billing/invoices
        [HttpGet("invoices")]
        public async Task<ActionResult<IEnumerable<InvoiceDto>>> GetInvoices(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                if (page < 1) return BadRequest("Page must be greater than 0.");
                if (pageSize < 1) return BadRequest("Page size must be greater than 0.");

                var invoices = await _repo.GetInvoicesAsync(page, pageSize);
                return Ok(invoices.Select(MapInvoice));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve invoices: {ex.Message}");
            }
        }

        // GET api/v1/billing/invoices/{id}
        [HttpGet("invoices/{id:guid}")]
        public async Task<ActionResult<InvoiceDto>> GetInvoiceById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Invoice ID cannot be empty.");

                var invoice = await _repo.GetInvoiceByIdAsync(id);
                if (invoice is null)
                    return NotFound($"Invoice with ID '{id}' was not found.");

                return Ok(MapInvoice(invoice));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve invoice: {ex.Message}");
            }
        }

        // POST api/v1/billing/invoices
        [HttpPost("invoices")]
        public async Task<ActionResult<InvoiceDto>> CreateInvoice([FromBody] CreateInvoiceDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Request body cannot be null.");

                var invoice = new Invoice
                {
                    PatientID        = dto.PatientID,
                    EncounterIDsJSON = dto.EncounterIDsJSON,
                    ChargesJSON      = "[]",
                    Currency         = dto.Currency,
                    DueDate          = dto.DueDate,
                    Status           = "Draft"
                };

                var created = await _repo.CreateInvoiceAsync(invoice);
                return CreatedAtAction(nameof(GetInvoiceById), new { id = created.InvoiceID }, MapInvoice(created));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create invoice: {ex.Message}");
            }
        }

        // ─────────────────────────────────────────────────────────
        // PAYMENTS
        // ─────────────────────────────────────────────────────────

        // GET api/v1/billing/payments
        [HttpGet("payments")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPayments(
            [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                if (page < 1) return BadRequest("Page must be greater than 0.");
                if (pageSize < 1) return BadRequest("Page size must be greater than 0.");

                var payments = await _repo.GetPaymentsAsync(page, pageSize);
                return Ok(payments.Select(MapPayment));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve payments: {ex.Message}");
            }
        }

        // POST api/v1/billing/payments
        [HttpPost("payments")]
        public async Task<ActionResult<PaymentDto>> CreatePayment([FromBody] CreatePaymentDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Request body cannot be null.");

                if (dto.Amount <= 0)
                    return BadRequest("Payment amount must be greater than 0.");

                var payment = new Payment
                {
                    InvoiceID = dto.InvoiceID,
                    PatientID = dto.PatientID,
                    Amount    = dto.Amount,
                    Method    = dto.Method
                };

                var created = await _repo.CreatePaymentAsync(payment);
                return CreatedAtAction(nameof(GetPayments), new { id = created.PaymentID }, MapPayment(created));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to create payment: {ex.Message}");
            }
        }

        // ─────────────────────────────────────────────────────────
        // MAPPERS
        // ─────────────────────────────────────────────────────────

        private static ChargeDto MapCharge(Charge c) => new()
        {
            ChargeID     = c.ChargeID,
            EncounterID  = c.EncounterID,
            CPTCodesJSON = c.CPTCodesJSON,
            ICDCodesJSON = c.ICDCodesJSON,
            Units        = c.Units,
            UnitPrice    = c.UnitPrice,
            Amount       = c.Amount,
            BilledAt     = c.BilledAt,
            Status       = c.Status
        };

        private static InvoiceDto MapInvoice(Invoice i) => new()
        {
            InvoiceID        = i.InvoiceID,
            PatientID        = i.PatientID,
            EncounterIDsJSON = i.EncounterIDsJSON,
            ChargesJSON      = i.ChargesJSON,
            Amount           = i.Amount,
            Currency         = i.Currency,
            IssuedAt         = i.IssuedAt,
            DueDate          = i.DueDate,
            Status           = i.Status,
            InvoiceURI       = i.InvoiceURI
        };

        private static PaymentDto MapPayment(Payment p) => new()
        {
            PaymentID = p.PaymentID,
            InvoiceID = p.InvoiceID,
            PatientID = p.PatientID,
            Amount    = p.Amount,
            Method    = p.Method,
            PaidAt    = p.PaidAt,
            Status    = p.Status
        };
    }
}
