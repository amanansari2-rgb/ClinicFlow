using ClinicFlow_Backend.DTO;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow_Backend.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class ReportingController : ControllerBase
    {
        private readonly IReportingRepository _repo;

        public ReportingController(IReportingRepository repo)
        {
            _repo = repo;
        }

        // GET api/v1/reports
        [HttpGet("reports")]
        public async Task<IActionResult> GetReports()
        {
            try
            {
                var reports = await _repo.GetReportsAsync();
                return Ok(reports.Select(MapReport));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST api/v1/reports
        [HttpPost("reports")]
        public async Task<IActionResult> PostReport([FromBody] CreateReportDto dto)
        {
            try
            {
                var report = new Report
                {
                    Scope          = dto.Scope,
                    ParametersJSON = dto.ParametersJSON,
                    MetricsJSON    = dto.MetricsJSON,
                    ReportURI      = dto.ReportURI,
                    GeneratedBy    = dto.GeneratedBy,
                    GeneratedAt    = DateTime.UtcNow
                };

                var created = await _repo.PostReportAsync(report);
                return CreatedAtAction(nameof(GetReports), new { id = created.ReportID }, MapReport(created));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET api/v1/kpis
        [HttpGet("kpis")]
        public async Task<IActionResult> GetKpis()
        {
            try
            {
                var kpis = await _repo.GetKpisAsync();
                return Ok(kpis.Select(MapKpi));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST api/v1/kpis
        [HttpPost("kpis")]
        public async Task<IActionResult> PostKpi([FromBody] CreateKpiDto dto)
        {
            try
            {
                var kpi = new KPI
                {
                    Name            = dto.Name,
                    Definition      = dto.Definition,
                    Target          = dto.Target,
                    CurrentValue    = dto.CurrentValue,
                    ReportingPeriod = dto.ReportingPeriod
                };

                var created = await _repo.PostKpiAsync(kpi);
                return CreatedAtAction(nameof(GetKpis), new { id = created.KPIID }, MapKpi(created));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // PUT api/v1/kpis/{id}
        [HttpPut("kpis/{id:guid}")]
        public async Task<IActionResult> PutKpi(Guid id, [FromBody] UpdateKpiDto dto)
        {
            try
            {
                var existing = await _repo.GetKpiAsync(id);
                if (existing is null)
                    return NotFound(new { error = $"KPI '{id}' not found." });

                existing.Name            = dto.Name;
                existing.Definition      = dto.Definition;
                existing.Target          = dto.Target;
                existing.CurrentValue    = dto.CurrentValue;
                existing.ReportingPeriod = dto.ReportingPeriod;

                await _repo.PutKpiAsync(id, existing);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET api/v1/audit-packages
        [HttpGet("audit-packages")]
        public async Task<IActionResult> GetAuditPackages()
        {
            try
            {
                var packages = await _repo.GetAuditPackagesAsync();
                return Ok(packages.Select(MapAudit));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST api/v1/audit-packages
        [HttpPost("audit-packages")]
        public async Task<IActionResult> PostAuditPackage([FromBody] CreateAuditPackageDto dto)
        {
            try
            {
                if (dto.PeriodEnd < dto.PeriodStart)
                    return BadRequest(new { error = "PeriodEnd must be on or after PeriodStart." });

                var package = new AuditPackage
                {
                    PeriodStart  = dto.PeriodStart,
                    PeriodEnd    = dto.PeriodEnd,
                    ContentsJSON = dto.ContentsJSON,
                    PackageURI   = dto.PackageURI,
                    GeneratedAt  = DateTime.UtcNow
                };

                var created = await _repo.PostAuditPackageAsync(package);
                return CreatedAtAction(nameof(GetAuditPackages), new { id = created.PackageID }, MapAudit(created));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ── Mappers ───────────────────────────────────────────────────────────

        private static ReportDto MapReport(Report r) => new()
        {
            ReportID       = r.ReportID,
            Scope          = r.Scope,
            ParametersJSON = r.ParametersJSON,
            MetricsJSON    = r.MetricsJSON,
            GeneratedBy    = r.GeneratedBy,
            GeneratedAt    = r.GeneratedAt,
            ReportURI      = r.ReportURI
        };

        private static KpiDto MapKpi(KPI k) => new()
        {
            KPIID           = k.KPIID,
            Name            = k.Name,
            Definition      = k.Definition,
            Target          = k.Target,
            CurrentValue    = k.CurrentValue,
            ReportingPeriod = k.ReportingPeriod
        };

        private static AuditPackageDto MapAudit(AuditPackage a) => new()
        {
            PackageID    = a.PackageID,
            PeriodStart  = a.PeriodStart,
            PeriodEnd    = a.PeriodEnd,
            ContentsJSON = a.ContentsJSON,
            GeneratedAt  = a.GeneratedAt,
            PackageURI   = a.PackageURI
        };
    }
}
