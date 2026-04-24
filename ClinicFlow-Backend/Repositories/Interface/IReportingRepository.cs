using ClinicFlow_Backend.Model;

namespace ClinicFlow_Backend.Repositories.Interface
{
    public interface IReportingRepository
    {
        // Reports
        Task<IEnumerable<Report>> GetReportsAsync();
        Task<Report> PostReportAsync(Report report);

        // KPIs
        Task<IEnumerable<KPI>> GetKpisAsync();
        Task<KPI?> GetKpiAsync(Guid id);
        Task<KPI> PostKpiAsync(KPI kpi);
        Task PutKpiAsync(Guid id, KPI kpi);

        // Audit Packages
        Task<IEnumerable<AuditPackage>> GetAuditPackagesAsync();
        Task<AuditPackage> PostAuditPackageAsync(AuditPackage package);
    }
}
