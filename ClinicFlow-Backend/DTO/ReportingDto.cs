namespace ClinicFlow_Backend.DTO
{
    // Returned by GET /api/v1/reports
    public class ReportDto
    {
        public Guid     ReportID       { get; set; }
        public string   Scope          { get; set; } = string.Empty;
        public string   ParametersJSON { get; set; } = string.Empty;
        public string?  MetricsJSON    { get; set; }
        public Guid     GeneratedBy    { get; set; }
        public DateTime GeneratedAt    { get; set; }
        public string?  ReportURI      { get; set; }
    }

    // Used by POST /api/v1/reports
    public class CreateReportDto
    {
        public string  Scope          { get; set; } = string.Empty;
        public string  ParametersJSON { get; set; } = string.Empty;
        public string? MetricsJSON    { get; set; }
        public string? ReportURI      { get; set; }
        public Guid    GeneratedBy    { get; set; }
    }

    // Returned by GET /api/v1/kpis
    public class KpiDto
    {
        public Guid     KPIID           { get; set; }
        public string   Name            { get; set; } = string.Empty;
        public string   Definition      { get; set; } = string.Empty;
        public decimal? Target          { get; set; }
        public decimal? CurrentValue    { get; set; }
        public string   ReportingPeriod { get; set; } = string.Empty;
    }

    // Used by POST /api/v1/kpis
    public class CreateKpiDto
    {
        public string   Name            { get; set; } = string.Empty;
        public string   Definition      { get; set; } = string.Empty;
        public decimal? Target          { get; set; }
        public decimal? CurrentValue    { get; set; }
        public string   ReportingPeriod { get; set; } = string.Empty;
    }

    // Used by PUT /api/v1/kpis/{id}
    public class UpdateKpiDto
    {
        public string   Name            { get; set; } = string.Empty;
        public string   Definition      { get; set; } = string.Empty;
        public decimal? Target          { get; set; }
        public decimal? CurrentValue    { get; set; }
        public string   ReportingPeriod { get; set; } = string.Empty;
    }

    // Returned by GET /api/v1/audit-packages
    public class AuditPackageDto
    {
        public Guid     PackageID    { get; set; }
        public DateOnly PeriodStart  { get; set; }
        public DateOnly PeriodEnd    { get; set; }
        public string   ContentsJSON { get; set; } = string.Empty;
        public DateTime GeneratedAt  { get; set; }
        public string?  PackageURI   { get; set; }
    }

    // Used by POST /api/v1/audit-packages
    public class CreateAuditPackageDto
    {
        public DateOnly PeriodStart  { get; set; }
        public DateOnly PeriodEnd    { get; set; }
        public string   ContentsJSON { get; set; } = string.Empty;
        public string?  PackageURI   { get; set; }
    }
}
