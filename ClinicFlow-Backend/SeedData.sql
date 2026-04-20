-- =============================================================================
-- ClinicFlow  —  SeedData.sql
-- Run this AFTER applying EF migrations (Update-Database)
-- Order matters: parent tables before child tables (FK constraints)
-- =============================================================================

-- ── 1. USERS ─────────────────────────────────────────────────────────────────
-- Covers all roles: Admin, Clinician, Scheduler, Billing, Auditor, Patient
-- PasswordHash is a placeholder (BCrypt hash of "Password@123")

INSERT INTO Users (UserID, Name, Role, Email, PasswordHash, Phone, Status, CreatedAt, UpdatedAt) VALUES
('A0000001-0000-0000-0000-000000000001', 'Admin User',       'Admin',     'admin@clinicflow.in',     '$2a$11$placeholder.hash.admin',     '9000000001', 'Active', GETUTCDATE(), GETUTCDATE()),
('A0000001-0000-0000-0000-000000000002', 'Dr. Arjun Mehta',  'Clinician', 'arjun@clinicflow.in',     '$2a$11$placeholder.hash.arjun',     '9000000002', 'Active', GETUTCDATE(), GETUTCDATE()),
('A0000001-0000-0000-0000-000000000003', 'Priya Scheduler',  'Scheduler', 'priya@clinicflow.in',     '$2a$11$placeholder.hash.priya',     '9000000003', 'Active', GETUTCDATE(), GETUTCDATE()),
('A0000001-0000-0000-0000-000000000004', 'Ravi Billing',     'Billing',   'ravi@clinicflow.in',      '$2a$11$placeholder.hash.ravi',      '9000000004', 'Active', GETUTCDATE(), GETUTCDATE()),
('A0000001-0000-0000-0000-000000000005', 'Karthika Auditor', 'Auditor',   'karthika@clinicflow.in',  '$2a$11$placeholder.hash.karthika',  '9000000005', 'Active', GETUTCDATE(), GETUTCDATE()),
('A0000001-0000-0000-0000-000000000006', 'Meera Patient',    'Patient',   'meera@gmail.com',         '$2a$11$placeholder.hash.meera',     '9000000006', 'Active', GETUTCDATE(), GETUTCDATE()),
('A0000001-0000-0000-0000-000000000007', 'Arun Patient',     'Patient',   'arun@gmail.com',          '$2a$11$placeholder.hash.arun',      '9000000007', 'Active', GETUTCDATE(), GETUTCDATE());

-- ── 2. CLINICS ────────────────────────────────────────────────────────────────

INSERT INTO Clinics (ClinicID, Name, AddressJSON, Status) VALUES
('B0000002-0000-0000-0000-000000000001', 'ClinicFlow Chennai Central', '{"street":"12 Anna Salai","city":"Chennai","state":"TN","postal":"600002"}', 'Active'),
('B0000002-0000-0000-0000-000000000002', 'ClinicFlow Adyar',           '{"street":"5 Lattice Bridge Rd","city":"Chennai","state":"TN","postal":"600020"}', 'Active');

-- ── 3. PROVIDERS ──────────────────────────────────────────────────────────────
-- Each provider links to a Clinician user

INSERT INTO Providers (ProviderID, UserID, Specialty, ClinicIDsJSON, AvailabilityJSON, MaxDailySlots, Status) VALUES
('C0000003-0000-0000-0000-000000000001',
 'A0000001-0000-0000-0000-000000000002',
 'General Medicine',
 '["B0000002-0000-0000-0000-000000000001"]',
 '{"mon":"09:00-17:00","tue":"09:00-17:00","wed":"09:00-17:00","thu":"09:00-17:00","fri":"09:00-13:00"}',
 20, 'Active');

-- ── 4. PATIENTS ───────────────────────────────────────────────────────────────
-- Each patient links to a Patient-role user

INSERT INTO Patients (PatientID, UserID, MRN, Name, DOB, Gender, ContactInfoJSON, AddressJSON, InsuranceInfoJSON, ConsentStatus, CreatedAt) VALUES
('D0000004-0000-0000-0000-000000000001',
 'A0000001-0000-0000-0000-000000000006',
 'MRN-0001', 'Meera Patient', '1990-05-15', 'Female',
 '{"phone":"9000000006","email":"meera@gmail.com","emergencyContact":"9000000099"}',
 '{"street":"10 Besant Nagar","city":"Chennai","state":"TN","postal":"600090"}',
 '{"payer":"Star Health","policyNumber":"SH123456","groupID":"GRP001"}',
 'Signed', GETUTCDATE()),

('D0000004-0000-0000-0000-000000000002',
 'A0000001-0000-0000-0000-000000000007',
 'MRN-0002', 'Arun Patient', '1985-11-20', 'Male',
 '{"phone":"9000000007","email":"arun@gmail.com","emergencyContact":"9000000088"}',
 '{"street":"7 T Nagar","city":"Chennai","state":"TN","postal":"600017"}',
 '{"payer":"HDFC Ergo","policyNumber":"HE789012","groupID":"GRP002"}',
 'Signed', GETUTCDATE());

-- ── 5. ROOMS ──────────────────────────────────────────────────────────────────

INSERT INTO Rooms (RoomID, ClinicID, Name, Capacity, ResourcesJSON, Status) VALUES
('E0000005-0000-0000-0000-000000000001', 'B0000002-0000-0000-0000-000000000001', 'Consultation Room 1', 1, '{"equipment":["ECG","BP Monitor"]}', 'Available'),
('E0000005-0000-0000-0000-000000000002', 'B0000002-0000-0000-0000-000000000001', 'Consultation Room 2', 1, '{"equipment":["Stethoscope","Thermometer"]}', 'Available');

-- ── 6. APPOINTMENTS ───────────────────────────────────────────────────────────
-- Needed so Encounters have a valid AppID FK

INSERT INTO Appointments (AppID, PatientID, ProviderID, ClinicID, ScheduledAt, DurationMinutes, Mode, Reason, Status, CreatedBy, CreatedAt) VALUES
('F0000006-0000-0000-0000-000000000001',
 'D0000004-0000-0000-0000-000000000001',
 'C0000003-0000-0000-0000-000000000001',
 'B0000002-0000-0000-0000-000000000001',
 '2025-04-20 10:00:00', 30, 'InPerson', 'Routine checkup', 'Completed',
 'A0000001-0000-0000-0000-000000000003', GETUTCDATE()),

('F0000006-0000-0000-0000-000000000002',
 'D0000004-0000-0000-0000-000000000002',
 'C0000003-0000-0000-0000-000000000001',
 'B0000002-0000-0000-0000-000000000001',
 '2025-04-21 11:00:00', 30, 'InPerson', 'Follow-up consultation', 'Scheduled',
 'A0000001-0000-0000-0000-000000000003', GETUTCDATE());

-- ── 7. AUDIT LOGS ─────────────────────────────────────────────────────────────
-- Required for AuditPackage contents to reference

INSERT INTO AuditLogs (AuditID, UserID, Action, ResourceType, ResourceID, Details, Timestamp) VALUES
('G0000007-0000-0000-0000-000000000001',
 'A0000001-0000-0000-0000-000000000003',
 'CREATE_APPOINTMENT', 'Appointment', 'F0000006-0000-0000-0000-000000000001',
 '{"action":"Appointment created for Meera Patient"}', GETUTCDATE()),

('G0000007-0000-0000-0000-000000000002',
 'A0000001-0000-0000-0000-000000000002',
 'UPDATE_APPOINTMENT', 'Appointment', 'F0000006-0000-0000-0000-000000000001',
 '{"action":"Status changed to Completed"}', GETUTCDATE());

-- =============================================================================
-- MODULE 5.7 — REPORTING, KPIs & AUDIT PACKAGES
-- Tests: GET/POST /api/v1/reports
--        GET/POST/PUT /api/v1/kpis
--        GET/POST /api/v1/audit-packages
-- =============================================================================

-- ── 8. REPORTS ────────────────────────────────────────────────────────────────
-- GeneratedBy must be an existing UserID

INSERT INTO Reports (ReportID, Scope, ParametersJSON, MetricsJSON, GeneratedBy, GeneratedAt, ReportURI) VALUES
('H0000008-0000-0000-0000-000000000001',
 'Operational',
 '{"dateFrom":"2025-04-01","dateTo":"2025-04-20","clinicID":"B0000002-0000-0000-0000-000000000001"}',
 '{"totalAppointments":42,"completed":38,"noShows":4}',
 'A0000001-0000-0000-0000-000000000001',
 GETUTCDATE(), NULL),

('H0000008-0000-0000-0000-000000000002',
 'Financial',
 '{"dateFrom":"2025-04-01","dateTo":"2025-04-20","clinicID":"B0000002-0000-0000-0000-000000000001"}',
 '{"totalBilled":125000.00,"totalCollected":98000.00,"outstanding":27000.00}',
 'A0000001-0000-0000-0000-000000000004',
 GETUTCDATE(), 'https://storage.clinicflow.in/reports/financial-apr-2025.pdf'),

('H0000008-0000-0000-0000-000000000003',
 'Clinical',
 '{"dateFrom":"2025-04-01","dateTo":"2025-04-20","providerID":"C0000003-0000-0000-0000-000000000001"}',
 '{"encounters":38,"avgDurationMinutes":28,"topDiagnosis":"Hypertension"}',
 'A0000001-0000-0000-0000-000000000002',
 GETUTCDATE(), NULL);

-- ── 9. KPIs ───────────────────────────────────────────────────────────────────

INSERT INTO KPIs (KPIID, Name, Definition, Target, CurrentValue, ReportingPeriod) VALUES
('I0000009-0000-0000-0000-000000000001',
 'Average Wait Time',
 'Mean time (minutes) from check-in to consultation start',
 15.00, 18.50, 'Daily'),

('I0000009-0000-0000-0000-000000000002',
 'No-Show Rate',
 'Percentage of scheduled appointments where patient did not attend',
 5.00, 9.52, 'Weekly'),

('I0000009-0000-0000-0000-000000000003',
 'Revenue Collection Rate',
 'Percentage of billed amount successfully collected',
 95.00, 78.40, 'Monthly');

-- ── 10. AUDIT PACKAGES ────────────────────────────────────────────────────────

INSERT INTO AuditPackages (PackageID, PeriodStart, PeriodEnd, ContentsJSON, GeneratedAt, PackageURI) VALUES
('J0000010-0000-0000-0000-000000000001',
 '2025-04-01', '2025-04-15',
 '{"auditLogIDs":["G0000007-0000-0000-0000-000000000001","G0000007-0000-0000-0000-000000000002"],"sections":["Appointments","Users"]}',
 GETUTCDATE(), NULL),

('J0000010-0000-0000-0000-000000000002',
 '2025-04-16', '2025-04-20',
 '{"auditLogIDs":["G0000007-0000-0000-0000-000000000002"],"sections":["Billing","Encounters"]}',
 GETUTCDATE(), 'https://storage.clinicflow.in/audit/apr16-20-2025.zip');

-- =============================================================================
-- MODULE 5.8 — NOTIFICATIONS, ALERTS & TASKS
-- Tests: GET /api/v1/notifications?userId=...
--        PATCH /api/v1/notifications/{id}/read
--        DELETE /api/v1/notifications/{id}
--        GET /api/v1/tasks?userId=...
--        POST /api/v1/tasks
--        PUT /api/v1/tasks/{id}
--        PATCH /api/v1/tasks/{id}/complete
-- =============================================================================

-- ── 11. NOTIFICATIONS ─────────────────────────────────────────────────────────
-- Mix of Unread, Read, and Dismissed to test all three PATCH/DELETE transitions

INSERT INTO Notifications (NotificationID, UserID, EntityID, Message, Category, Severity, CreatedAt, ReadAt, Status) VALUES

-- Unread — test PATCH /{id}/read
('K0000011-0000-0000-0000-000000000001',
 'A0000001-0000-0000-0000-000000000002',
 'F0000006-0000-0000-0000-000000000002',
 'You have an upcoming appointment with Arun Patient tomorrow at 11:00 AM.',
 'Appointment', 'Info', GETUTCDATE(), NULL, 'Unread'),

-- Unread — test DELETE /{id} (Dismiss)
('K0000011-0000-0000-0000-000000000002',
 'A0000001-0000-0000-0000-000000000004',
 NULL,
 'Invoice INV-2025-042 is overdue by 7 days. Outstanding: ₹27,000.',
 'Billing', 'Warning', GETUTCDATE(), NULL, 'Unread'),

-- Unread — for Scheduler user
('K0000011-0000-0000-0000-000000000003',
 'A0000001-0000-0000-0000-000000000003',
 NULL,
 'No-Show Rate has exceeded the 5% target this week (current: 9.52%).',
 'System', 'Critical', GETUTCDATE(), NULL, 'Unread'),

-- Already Read — to verify GET returns mixed statuses
('K0000011-0000-0000-0000-000000000004',
 'A0000001-0000-0000-0000-000000000002',
 'H0000008-0000-0000-0000-000000000001',
 'Operational report for April 1–20 is ready to view.',
 'System', 'Info', DATEADD(HOUR, -2, GETUTCDATE()), DATEADD(HOUR, -1, GETUTCDATE()), 'Read'),

-- Dismissed — to verify GET returns all statuses
('K0000011-0000-0000-0000-000000000005',
 'A0000001-0000-0000-0000-000000000002',
 NULL,
 'System maintenance scheduled for Sunday 02:00–04:00 AM.',
 'System', 'Info', DATEADD(DAY, -1, GETUTCDATE()), DATEADD(DAY, -1, GETUTCDATE()), 'Dismissed');

-- ── 12. CLINIC TASKS ──────────────────────────────────────────────────────────
-- Mix of Open, InProgress, and Completed to test all endpoints

INSERT INTO ClinicTasks (TaskID, AssignedTo, RelatedEntityID, Description, DueDate, Priority, CreatedAt, CompletedAt, Status) VALUES

-- Open — test PUT /{id} and PATCH /{id}/complete
('L0000012-0000-0000-0000-000000000001',
 'A0000001-0000-0000-0000-000000000003',
 'F0000006-0000-0000-0000-000000000002',
 'Confirm appointment slot for Arun Patient and send reminder SMS.',
 DATEADD(DAY, 1, GETUTCDATE()), 'High',
 GETUTCDATE(), NULL, 'Open'),

-- Open — low priority, test PUT to change priority
('L0000012-0000-0000-0000-000000000002',
 'A0000001-0000-0000-0000-000000000004',
 NULL,
 'Reconcile April billing statements and update payment records.',
 DATEADD(DAY, 3, GETUTCDATE()), 'Medium',
 GETUTCDATE(), NULL, 'Open'),

-- InProgress — test PATCH /{id}/complete
('L0000012-0000-0000-0000-000000000003',
 'A0000001-0000-0000-0000-000000000002',
 'H0000008-0000-0000-0000-000000000002',
 'Review financial report for April and flag discrepancies.',
 DATEADD(DAY, 2, GETUTCDATE()), 'High',
 GETUTCDATE(), NULL, 'InProgress'),

-- Already Completed — test PATCH /{id}/complete returns 409 Conflict
('L0000012-0000-0000-0000-000000000004',
 'A0000001-0000-0000-0000-000000000005',
 'J0000010-0000-0000-0000-000000000001',
 'Export and archive audit package for Apr 1–15.',
 DATEADD(DAY, -1, GETUTCDATE()), 'Critical',
 DATEADD(DAY, -2, GETUTCDATE()), DATEADD(HOUR, -3, GETUTCDATE()), 'Completed'),

-- Overdue — test GET returns overdue tasks
('L0000012-0000-0000-0000-000000000005',
 'A0000001-0000-0000-0000-000000000003',
 NULL,
 'Update provider availability schedule for next month.',
 DATEADD(DAY, -3, GETUTCDATE()), 'Medium',
 DATEADD(DAY, -5, GETUTCDATE()), NULL, 'Overdue');

-- =============================================================================
-- END OF SEED DATA
-- =============================================================================
