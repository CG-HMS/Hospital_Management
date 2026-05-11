using FluentValidation;
using FluentValidation.AspNetCore;
using Hms.API.Data;
using Hms.API.Repository;
using Hms.API.Services;
using Hms.API.Validator;
using Microsoft.EntityFrameworkCore;

namespace Hms.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            
            // Add Swagger/OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
