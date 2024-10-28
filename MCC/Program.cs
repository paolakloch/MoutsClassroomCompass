
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using MCC.Data;
using MCC.Services;
using MCC.Repositories;

namespace MCC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("StringConnection")));

            builder.Services.AddScoped<StudentService>();
            builder.Services.AddScoped<StudentRepository>();

            builder.Services.AddScoped<SubjectService>();
            builder.Services.AddScoped<SubjectRepository>();

            builder.Services.AddScoped<GradeService>();
            builder.Services.AddScoped<GradeRepository>();

            builder.Services.AddScoped<TeacherService>();
            builder.Services.AddScoped<TeacherRepository>();

            builder.Services.AddScoped<ValidationService>();

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
