using FluentValidation;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.AspNetCore;
using Hms.API.Data;
using Hms.API.DTOs.Medication;
using Hms.API.DTOs.Patient;
using Hms.API.Mapping;
using Hms.API.Middleware;
using Hms.API.Repository;
using Hms.API.Repository;
using Hms.API.Repository.Interfaces;
using Hms.API.Services;
using Hms.API.Services.Interfaces;
using Hms.API.Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Hms.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ── Database ───────────────────────────────────────────────────────
            builder.Services.AddDbContext<MyAppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // ── JWT Authentication ─────────────────────────────────────────────
            var jwt = builder.Configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwt["Secret"]!);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwt["Issuer"],
                        ValidAudience = jwt["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization();

            // ── FluentValidation ───────────────────────────────────────────────
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            // ── Repositories ───────────────────────────────────────────────────
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();

            // ── Services ───────────────────────────────────────────────────────
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRoomService, RoomService>();

            // ── Controllers + Swagger ──────────────────────────────────────────
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter: Bearer {token}"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddDbContext<Data.MyAppDbContext>();
            builder.Services.AddScoped<INurseRepository, NurseRepository>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<Services.INurseService, Services.NurseService>();
            builder.Services.AddScoped<Services.IAppointmentService, Services.AppointmentService>();
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<NurseDtoValidator>();

            // Repositories & Services
            builder.Services.AddScoped<IPhysicianRepository, PhysicianRepository>();
            builder.Services.AddScoped<IProcedureRepository, ProcedureRepository>();
            builder.Services.AddScoped<IPhysicianService, PhysicianService>();
            builder.Services.AddScoped<IProcedureService, ProcedureService>();

            // FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<DtoValidators.CreatePhysicianDtoValidator>();

            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IMedicationRepository, MedicationRepository>();

            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IMedicationService, MedicationService>();

            builder.Services.AddFluentValidationAutoValidation();

            builder.Services.AddScoped<IValidator<PatientRequestDto>, PatientValidator>();

            builder.Services.AddScoped<IValidator<MedicationRequestDto>, MedicationValidator>();


            builder.Services.AddAutoMapper(typeof(MedicationProfile));

            // Add AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            // Add DbContext
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                "Server=.;Database=HospitalManagementSystem;Trusted_Connection=true;Encrypt=false;";
            builder.Services.AddDbContext<MyAppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add Repositories
            builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            builder.Services.AddScoped<IStayRepository, StayRepository>();

            // Add Services
            builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
            builder.Services.AddScoped<IStayService, StayService>();

            var app = builder.Build();

            // ── Middleware Pipeline ────────────────────────────────────────────
            app.UseMiddleware<ExceptionMiddleware>(); // ← must be first

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<Middleware.ExceptionHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseAuthentication(); // ← must come before UseAuthorization
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}