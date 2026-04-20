using ClinicFlow_Backend.Data;
using ClinicFlow_Backend.Repositories.Implementation;
using ClinicFlow_Backend.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
<<<<<<< HEAD
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<ISchedulingRepository, SchedulingRepository>();
=======
builder.Services.AddScoped<IClinicRepository, ClinicRepository>();
>>>>>>> 7c35a29f35aeac2466518a8215209f56e4f09dd7

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
