using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicFlow_Backend.Migrations
{
    /// <inheritdoc />
    public partial class CreatingModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_identity",
                table: "identity");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "identity");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "identity");

            migrationBuilder.RenameTable(
                name: "identity",
                newName: "Identities");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Identities",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "UserID",
                table: "Identities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Identities",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Identities",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Identities",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Identities",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Identities",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Identities",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Active");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Identities",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Identities",
                table: "Identities",
                column: "UserID");

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    AuditID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ResourceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResourceID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.AuditID);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Identities_UserID",
                        column: x => x.UserID,
                        principalTable: "Identities",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditPackages",
                columns: table => new
                {
                    PackageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PeriodStart = table.Column<DateOnly>(type: "date", nullable: false),
                    PeriodEnd = table.Column<DateOnly>(type: "date", nullable: false),
                    ContentsJSON = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PackageURI = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditPackages", x => x.PackageID);
                });

            migrationBuilder.CreateTable(
                name: "Clinics",
                columns: table => new
                {
                    ClinicID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AddressJSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinics", x => x.ClinicID);
                });

            migrationBuilder.CreateTable(
                name: "ClinicTasks",
                columns: table => new
                {
                    TaskID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedTo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RelatedEntityID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Medium"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Open")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicTasks", x => x.TaskID);
                    table.ForeignKey(
                        name: "FK_ClinicTasks_Identities_AssignedTo",
                        column: x => x.AssignedTo,
                        principalTable: "Identities",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KPIs",
                columns: table => new
                {
                    KPIID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Definition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Target = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CurrentValue = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ReportingPeriod = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KPIs", x => x.KPIID);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Info"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Unread")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationID);
                    table.ForeignKey(
                        name: "FK_Notifications_Identities_UserID",
                        column: x => x.UserID,
                        principalTable: "Identities",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MRN = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DOB = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ContactInfoJSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressJSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceInfoJSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConsentStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientID);
                    table.ForeignKey(
                        name: "FK_Patients_Identities_UserID",
                        column: x => x.UserID,
                        principalTable: "Identities",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ParametersJSON = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetricsJSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneratedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ReportURI = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportID);
                    table.ForeignKey(
                        name: "FK_Reports_Identities_GeneratedBy",
                        column: x => x.GeneratedBy,
                        principalTable: "Identities",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    ProviderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClinicIDsJSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvailabilityJSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxDailySlots = table.Column<int>(type: "int", nullable: false, defaultValue: 20),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ClinicID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.ProviderID);
                    table.ForeignKey(
                        name: "FK_Providers_Clinics_ClinicID",
                        column: x => x.ClinicID,
                        principalTable: "Clinics",
                        principalColumn: "ClinicID");
                    table.ForeignKey(
                        name: "FK_Providers_Identities_UserID",
                        column: x => x.UserID,
                        principalTable: "Identities",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    ResourcesJSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Available")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomID);
                    table.ForeignKey(
                        name: "FK_Rooms_Clinics_ClinicID",
                        column: x => x.ClinicID,
                        principalTable: "Clinics",
                        principalColumn: "ClinicID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntakeForms",
                columns: table => new
                {
                    FormID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisitReason = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    SymptomsJSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllergiesJSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicationsJSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Portal"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Draft")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntakeForms", x => x.FormID);
                    table.ForeignKey(
                        name: "FK_IntakeForms_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncounterIDsJSON = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChargesJSON = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, defaultValue: "USD"),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Draft"),
                    InvoiceURI = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceID);
                    table.ForeignKey(
                        name: "FK_Invoices_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    AppID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false, defaultValue: 30),
                    Mode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Scheduled"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.AppID);
                    table.ForeignKey(
                        name: "FK_Appointments_Clinics_ClinicID",
                        column: x => x.ClinicID,
                        principalTable: "Clinics",
                        principalColumn: "ClinicID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Providers_ProviderID",
                        column: x => x.ProviderID,
                        principalTable: "Providers",
                        principalColumn: "ProviderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Waitlists",
                columns: table => new
                {
                    WaitID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestedWindowStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedWindowEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Active")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Waitlists", x => x.WaitID);
                    table.ForeignKey(
                        name: "FK_Waitlists_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Waitlists_Providers_ProviderID",
                        column: x => x.ProviderID,
                        principalTable: "Providers",
                        principalColumn: "ProviderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvoiceID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pending")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_Payments_Invoices_InvoiceID",
                        column: x => x.InvoiceID,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Encounters",
                columns: table => new
                {
                    EncounterID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PatientID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VisitType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ChiefComplaint = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EncounterNoteURI = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "InProgress")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encounters", x => x.EncounterID);
                    table.ForeignKey(
                        name: "FK_Encounters_Appointments_AppID",
                        column: x => x.AppID,
                        principalTable: "Appointments",
                        principalColumn: "AppID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Encounters_Patients_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Patients",
                        principalColumn: "PatientID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Encounters_Providers_ProviderID",
                        column: x => x.ProviderID,
                        principalTable: "Providers",
                        principalColumn: "ProviderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Charges",
                columns: table => new
                {
                    ChargeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncounterID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CPTCodesJSON = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ICDCodesJSON = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Units = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    UnitPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    BilledAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Draft")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charges", x => x.ChargeID);
                    table.ForeignKey(
                        name: "FK_Charges_Encounters_EncounterID",
                        column: x => x.EncounterID,
                        principalTable: "Encounters",
                        principalColumn: "EncounterID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncounterID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    OrderDetailsJSON = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ExpectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Ordered")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_Encounters_EncounterID",
                        column: x => x.EncounterID,
                        principalTable: "Encounters",
                        principalColumn: "EncounterID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Identities_OrderedBy",
                        column: x => x.OrderedBy,
                        principalTable: "Identities",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Referrals",
                columns: table => new
                {
                    ReferralID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EncounterID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromProvider = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToProviderInfoJSON = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AppointmentID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Sent")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referrals", x => x.ReferralID);
                    table.ForeignKey(
                        name: "FK_Referrals_Appointments_AppointmentID",
                        column: x => x.AppointmentID,
                        principalTable: "Appointments",
                        principalColumn: "AppID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Referrals_Encounters_EncounterID",
                        column: x => x.EncounterID,
                        principalTable: "Encounters",
                        principalColumn: "EncounterID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Referrals_Providers_FromProvider",
                        column: x => x.FromProvider,
                        principalTable: "Providers",
                        principalColumn: "ProviderID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Identities_Email",
                table: "Identities",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ClinicID",
                table: "Appointments",
                column: "ClinicID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientID",
                table: "Appointments",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ProviderID",
                table: "Appointments",
                column: "ProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserID",
                table: "AuditLogs",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Charges_EncounterID",
                table: "Charges",
                column: "EncounterID");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicTasks_AssignedTo",
                table: "ClinicTasks",
                column: "AssignedTo");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_AppID",
                table: "Encounters",
                column: "AppID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_PatientID",
                table: "Encounters",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Encounters_ProviderID",
                table: "Encounters",
                column: "ProviderID");

            migrationBuilder.CreateIndex(
                name: "IX_IntakeForms_PatientID",
                table: "IntakeForms",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PatientID",
                table: "Invoices",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserID",
                table: "Notifications",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EncounterID",
                table: "Orders",
                column: "EncounterID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderedBy",
                table: "Orders",
                column: "OrderedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_MRN",
                table: "Patients",
                column: "MRN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UserID",
                table: "Patients",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_InvoiceID",
                table: "Payments",
                column: "InvoiceID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PatientID",
                table: "Payments",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_ClinicID",
                table: "Providers",
                column: "ClinicID");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_UserID",
                table: "Providers",
                column: "UserID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_AppointmentID",
                table: "Referrals",
                column: "AppointmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_EncounterID",
                table: "Referrals",
                column: "EncounterID");

            migrationBuilder.CreateIndex(
                name: "IX_Referrals_FromProvider",
                table: "Referrals",
                column: "FromProvider");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_GeneratedBy",
                table: "Reports",
                column: "GeneratedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_ClinicID",
                table: "Rooms",
                column: "ClinicID");

            migrationBuilder.CreateIndex(
                name: "IX_Waitlists_PatientID",
                table: "Waitlists",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_Waitlists_ProviderID",
                table: "Waitlists",
                column: "ProviderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "AuditPackages");

            migrationBuilder.DropTable(
                name: "Charges");

            migrationBuilder.DropTable(
                name: "ClinicTasks");

            migrationBuilder.DropTable(
                name: "IntakeForms");

            migrationBuilder.DropTable(
                name: "KPIs");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Referrals");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Waitlists");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Encounters");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropTable(
                name: "Clinics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Identities",
                table: "Identities");

            migrationBuilder.DropIndex(
                name: "IX_Identities_Email",
                table: "Identities");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Identities");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Identities");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Identities");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Identities");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Identities");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Identities");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Identities");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Identities");

            migrationBuilder.RenameTable(
                name: "Identities",
                newName: "identity");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "identity",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "identity",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "identity",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_identity",
                table: "identity",
                column: "Id");
        }
    }
}
