using ClinicFlow_Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow_Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ── 5.1 Identity & Access Management ────────────────────────────
        public DbSet<Identity> Identities { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        // ── 5.2 Patient Registry & Intake ───────────────────────────────
        public DbSet<Patient> Patients { get; set; }
        public DbSet<IntakeForm> IntakeForms { get; set; }

        // ── 5.3 Appointment Scheduling & Calendar ────────────────────────
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Waitlist> Waitlists { get; set; }

        // ── 5.4 Encounter Documentation & Resources ──────────────────────
        public DbSet<Encounter> Encounters { get; set; }
        public DbSet<Room> Rooms { get; set; }

        // ── 5.5 Orders & Referral Tracking ──────────────────────────────
        public DbSet<Order> Orders { get; set; }
        public DbSet<Referral> Referrals { get; set; }

        // ── 5.6 Billing & Revenue Capture ────────────────────────────────
        public DbSet<Charge> Charges { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Payment> Payments { get; set; }

        // ── 5.7 Reporting, KPIs & Audit Packages ─────────────────────────
        public DbSet<Report> Reports { get; set; }
        public DbSet<KPI> KPIs { get; set; }
        public DbSet<AuditPackage> AuditPackages { get; set; }

        // ── 5.8 Notifications, Alerts & Task Lists ───────────────────────
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ClinicTask> ClinicTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Identity ─────────────────────────────────────────────────
            modelBuilder.Entity<Identity>(e =>
            {
                e.HasIndex(x => x.Email).IsUnique();
                e.Property(x => x.Status).HasDefaultValue("Active");
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.Property(x => x.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // ── AuditLog (append-only — no update/delete) ────────────────
            modelBuilder.Entity<AuditLog>(e =>
            {
                e.Property(x => x.Timestamp).HasDefaultValueSql("GETUTCDATE()");
                e.HasOne(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.UserID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Patient ──────────────────────────────────────────────────
            modelBuilder.Entity<Patient>(e =>
            {
                e.HasIndex(x => x.MRN).IsUnique();
                e.Property(x => x.ConsentStatus).HasDefaultValue("Pending");
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasOne(x => x.User)
                 .WithOne()
                 .HasForeignKey<Patient>(x => x.UserID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── IntakeForm ───────────────────────────────────────────────
            modelBuilder.Entity<IntakeForm>(e =>
            {
                e.Property(x => x.Source).HasDefaultValue("Portal");
                e.Property(x => x.Status).HasDefaultValue("Draft");
                e.HasOne(x => x.Patient)
                 .WithMany(p => p.IntakeForms)
                 .HasForeignKey(x => x.PatientID)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Clinic ───────────────────────────────────────────────────
            modelBuilder.Entity<Clinic>(e =>
            {
                e.Property(x => x.Status).HasDefaultValue("Active");
            });

            // ── Provider ─────────────────────────────────────────────────
            modelBuilder.Entity<Provider>(e =>
            {
                e.Property(x => x.MaxDailySlots).HasDefaultValue(20);
                e.HasOne(x => x.User)
                 .WithOne()
                 .HasForeignKey<Provider>(x => x.UserID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Appointment ──────────────────────────────────────────────
            modelBuilder.Entity<Appointment>(e =>
            {
                e.Property(x => x.DurationMinutes).HasDefaultValue(30);
                e.Property(x => x.Status).HasDefaultValue("Scheduled");
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasOne(x => x.Patient)
                 .WithMany(p => p.Appointments)
                 .HasForeignKey(x => x.PatientID)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Provider)
                 .WithMany(p => p.Appointments)
                 .HasForeignKey(x => x.ProviderID)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Clinic)
                 .WithMany(c => c.Appointments)
                 .HasForeignKey(x => x.ClinicID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Waitlist ─────────────────────────────────────────────────
            modelBuilder.Entity<Waitlist>(e =>
            {
                e.Property(x => x.Status).HasDefaultValue("Active");
                e.HasOne(x => x.Patient)
                 .WithMany(p => p.Waitlists)
                 .HasForeignKey(x => x.PatientID)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Provider)
                 .WithMany(p => p.Waitlists)
                 .HasForeignKey(x => x.ProviderID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Encounter ────────────────────────────────────────────────
            modelBuilder.Entity<Encounter>(e =>
            {
                e.Property(x => x.Status).HasDefaultValue("InProgress");
                e.HasOne(x => x.Appointment)
                 .WithOne(a => a.Encounter)
                 .HasForeignKey<Encounter>(x => x.AppID)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Patient)
                 .WithMany(p => p.Encounters)
                 .HasForeignKey(x => x.PatientID)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Provider)
                 .WithMany(p => p.Encounters)
                 .HasForeignKey(x => x.ProviderID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Room ─────────────────────────────────────────────────────
            modelBuilder.Entity<Room>(e =>
            {
                e.Property(x => x.Capacity).HasDefaultValue(1);
                e.Property(x => x.Status).HasDefaultValue("Available");
                e.HasOne(x => x.Clinic)
                 .WithMany(c => c.Rooms)
                 .HasForeignKey(x => x.ClinicID)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Order ────────────────────────────────────────────────────
            modelBuilder.Entity<Order>(e =>
            {
                e.Property(x => x.Status).HasDefaultValue("Ordered");
                e.Property(x => x.OrderedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasOne(x => x.Encounter)
                 .WithMany(en => en.Orders)
                 .HasForeignKey(x => x.EncounterID)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.OrderedByUser)
                 .WithMany()
                 .HasForeignKey(x => x.OrderedBy)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Referral ─────────────────────────────────────────────────
            modelBuilder.Entity<Referral>(e =>
            {
                e.Property(x => x.Status).HasDefaultValue("Sent");
                e.HasOne(x => x.Encounter)
                 .WithMany(en => en.Referrals)
                 .HasForeignKey(x => x.EncounterID)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.FromProviderNav)
                 .WithMany(p => p.Referrals)
                 .HasForeignKey(x => x.FromProvider)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Appointment)
                 .WithMany()
                 .HasForeignKey(x => x.AppointmentID)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // ── Charge ───────────────────────────────────────────────────
            modelBuilder.Entity<Charge>(e =>
            {
                e.Property(x => x.Units).HasDefaultValue(1);
                e.Property(x => x.Status).HasDefaultValue("Draft");
                e.Property(x => x.BilledAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasOne(x => x.Encounter)
                 .WithMany(en => en.Charges)
                 .HasForeignKey(x => x.EncounterID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Invoice ──────────────────────────────────────────────────
            modelBuilder.Entity<Invoice>(e =>
            {
                e.Property(x => x.Currency).HasDefaultValue("USD");
                e.Property(x => x.Status).HasDefaultValue("Draft");
                e.Property(x => x.IssuedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasOne(x => x.Patient)
                 .WithMany(p => p.Invoices)
                 .HasForeignKey(x => x.PatientID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Payment ──────────────────────────────────────────────────
            modelBuilder.Entity<Payment>(e =>
            {
                e.Property(x => x.Status).HasDefaultValue("Pending");
                e.Property(x => x.PaidAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasOne(x => x.Invoice)
                 .WithMany(i => i.Payments)
                 .HasForeignKey(x => x.InvoiceID)
                 .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(x => x.Patient)
                 .WithMany(p => p.Payments)
                 .HasForeignKey(x => x.PatientID)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Report ───────────────────────────────────────────────────
            modelBuilder.Entity<Report>(e =>
            {
                e.Property(x => x.GeneratedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasOne(x => x.GeneratedByUser)
                 .WithMany()
                 .HasForeignKey(x => x.GeneratedBy)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Notification ─────────────────────────────────────────────
            modelBuilder.Entity<Notification>(e =>
            {
                e.Property(x => x.Status).HasDefaultValue("Unread");
                e.Property(x => x.Severity).HasDefaultValue("Info");
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasOne(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.UserID)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // ── ClinicTask ───────────────────────────────────────────────
            modelBuilder.Entity<ClinicTask>(e =>
            {
                e.Property(x => x.Priority).HasDefaultValue("Medium");
                e.Property(x => x.Status).HasDefaultValue("Open");
                e.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                e.HasOne(x => x.AssignedToUser)
                 .WithMany()
                 .HasForeignKey(x => x.AssignedTo)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
