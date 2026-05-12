using FluentValidation;
using FluentValidation.AspNetCore;

using Hms.API.Data;

using Hms.API.DTOs.Patient;
using Hms.API.DTOs.Medication;

using Hms.API.Mapping;

using Hms.API.Repository;
using Hms.API.Repository.Interfaces;

using Hms.API.Services;
using Hms.API.Services.Interfaces;

using Hms.API.Validator;

using Microsoft.EntityFrameworkCore;
using Hms.API.Middleware;
namespace Hms.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<MyAppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                )
            );

            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IMedicationRepository, MedicationRepository>();

            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IMedicationService, MedicationService>();

            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddScoped<IValidator<PatientRequestDto>, PatientValidator>();

            builder.Services.AddScoped<IValidator<MedicationRequestDto>, MedicationValidator>();

            
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}