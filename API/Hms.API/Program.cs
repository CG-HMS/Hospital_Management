using FluentValidation;
using FluentValidation.AspNetCore;
using Hms.API.Data;
using Hms.API.Middleware;
using Hms.API.Repository;
using Hms.API.Repository.Interfaces;
using Hms.API.Services;
using Hms.API.Services.Interfaces;
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

            // ── FluentValidation ── one call scans every validator in the assembly
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();

            // ── AutoMapper ── one call scans every profile in the assembly
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            // ── Repositories ───────────────────────────────────────────────────
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<INurseRepository, NurseRepository>();
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            builder.Services.AddScoped<IPhysicianRepository, PhysicianRepository>();
            builder.Services.AddScoped<IProcedureRepository, ProcedureRepository>();
            builder.Services.AddScoped<IPatientRepository, PatientRepository>();
            builder.Services.AddScoped<IMedicationRepository, MedicationRepository>();
            builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            builder.Services.AddScoped<IStayRepository, StayRepository>();

            // ── Services ───────────────────────────────────────────────────────
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<INurseService, NurseService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IPhysicianService, PhysicianService>();
            builder.Services.AddScoped<IProcedureService, ProcedureService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IMedicationService, MedicationService>();
            builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
            builder.Services.AddScoped<IStayService, StayService>();

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
                    Description = "Enter: Bearer {your-token}"
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

            var app = builder.Build();

            // ── Middleware Pipeline (ORDER MATTERS) ────────────────────────────
            app.UseMiddleware<RequestLoggingMiddleware>(); 
            app.UseMiddleware<ExceptionMiddleware>();    

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication(); 
            app.UseAuthorization();   
            app.MapControllers();

            app.Run();
        }
    }
}