using ClinicFlow_Backend.Data;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow_Backend.Repositories.Implementation
{
    public class ReportingRepository : IReportingRepository
    {
        private readonly AppDbContext _context;

        public ReportingRepository(AppDbContext context)
        {
            _context = context;
        }

        // ── Reports ───────────────────────────────────────────────────────────

        public async Task<IEnumerable<Report>> GetReportsAsync()
        {
            return await _context.Reports
                .OrderByDescending(r => r.GeneratedAt)
                .ToListAsync();
        }

        public async Task<Report> PostReportAsync(Report report)
        {
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            return report;
        }

        // ── KPIs ──────────────────────────────────────────────────────────────

        public async Task<IEnumerable<KPI>> GetKpisAsync()
        {
            return await _context.KPIs
                .OrderBy(k => k.Name)
                .ToListAsync();
        }

        public async Task<KPI?> GetKpiAsync(Guid id)
        {
            return await _context.KPIs.FindAsync(id);
        }

        public async Task<KPI> PostKpiAsync(KPI kpi)
        {
            _context.KPIs.Add(kpi);
            await _context.SaveChangesAsync();
            return kpi;
        }

        public async Task PutKpiAsync(Guid id, KPI kpi)
        {
            _context.Entry(kpi).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // ── Audit Packages ────────────────────────────────────────────────────

        public async Task<IEnumerable<AuditPackage>> GetAuditPackagesAsync()
        {
            return await _context.AuditPackages
                .OrderByDescending(a => a.GeneratedAt)
                .ToListAsync();
        }

        public async Task<AuditPackage> PostAuditPackageAsync(AuditPackage package)
        {
            _context.AuditPackages.Add(package);
            await _context.SaveChangesAsync();
            return package;
        }
    }
}
