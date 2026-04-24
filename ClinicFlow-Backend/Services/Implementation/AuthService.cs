using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ClinicFlow_Backend.Data;
using ClinicFlow_Backend.DTO;
using ClinicFlow_Backend.Model;
using ClinicFlow_Backend.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ClinicFlow_Backend.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // ── REGISTER ──────────────────────────────────────────────────────
        // Public — only ever creates a Patient account
        // Creates two records: User (login) + Patient (clinical)
        public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            // Check email not already taken
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == dto.Email);

            if (emailExists)
                throw new InvalidOperationException("An account with this email already exists.");

            // Step 1 — Create the User login record
            // Role is ALWAYS hardcoded to Patient — never from the request
            var user = new User
            {
                Name         = dto.Name.Trim(),
                Email        = dto.Email.Trim().ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Phone        = dto.Phone?.Trim(),
                Role         = "Patient",
                Status       = "Active",
                CreatedAt    = DateTime.UtcNow,
                UpdatedAt    = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Step 2 — Generate a unique MRN for this patient
            // Format: CF-YYYYMMDD-XXXX (e.g. CF-20260423-A3F9)
            // Loop ensures uniqueness even if two patients register at the same second
            string mrn;
            do
            {
                mrn = GenerateMRN();
            }
            while (await _context.Patients.AnyAsync(p => p.MRN == mrn));

            // Step 3 — Create the Patient clinical record linked to the User
            var patient = new Patient
            {
                UserID           = user.UserID,
                MRN              = mrn,
                Name             = user.Name,
                DOB              = dto.DOB,
                Gender           = dto.Gender.Trim(),
                ContactInfoJSON  = dto.ContactInfoJSON,
                AddressJSON      = dto.AddressJSON,
                InsuranceInfoJSON = dto.InsuranceInfoJSON,
                ConsentStatus    = "Pending",  // always starts as Pending
                CreatedAt        = DateTime.UtcNow
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            // Step 4 — Log them in immediately — no separate login step needed
            var accessToken = GenerateJwt(user);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                Role        = user.Role,
                Name        = user.Name
            };
        }

        // ── LOGIN ─────────────────────────────────────────────────────────
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            // Email already trimmed + lowercased by controller
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Status == "Active");

            // Generic error — never reveal if email exists or account is locked
            bool passwordValid;
            try { passwordValid = user != null && BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash); }
            catch { passwordValid = false; }

            if (user == null || !passwordValid)
                throw new UnauthorizedAccessException("Invalid credentials.");

            var accessToken = GenerateJwt(user);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                Role        = user.Role,
                Name        = user.Name
            };
        }

        // ── LOGOUT ────────────────────────────────────────────────────────
        // Phase 1: client-side only
        // Phase 2: refresh token revocation goes here
        public Task LogoutAsync()
        {
            return Task.CompletedTask;
        }

        // ── PRIVATE: Generate unique MRN ─────────────────────────────────
        // Format: CF-YYYYMMDD-XXXX
        // Example: CF-20260423-A3F9
        private static string GenerateMRN()
        {
            var date   = DateTime.UtcNow.ToString("yyyyMMdd");
            var random = Guid.NewGuid().ToString("N")[..4].ToUpper();
            return $"CF-{date}-{random}";
        }

        // ── PRIVATE: Build and sign the JWT ──────────────────────────────
        private string GenerateJwt(User user)
        {
            var secret   = _config["Jwt:Secret"];
            var issuer   = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                throw new InvalidOperationException("JWT configuration is missing. Check appsettings.");

            var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Email,          user.Email),
                new Claim(ClaimTypes.Role,           user.Role),
                new Claim(ClaimTypes.Name,           user.Name)
            };

            var token = new JwtSecurityToken(
                issuer:             issuer,
                audience:           audience,
                claims:             claims,
                expires:            DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
