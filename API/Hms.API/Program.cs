
using Hms.API.Data;
using Hms.API.Mapping;
using Hms.API.Middleware;
using Hms.API.Repository;
using Hms.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Hms.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MyAppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IPhysicianRepository, PhysicianRepository>();

            builder.Services.AddScoped<IPhysicianService, PhysicianService>();
            builder.Services.AddScoped<IProcedureRepository, ProcedureRepository>();

            builder.Services.AddScoped<IProcedureService, ProcedureService>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
