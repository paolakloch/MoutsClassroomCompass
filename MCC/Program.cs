using Microsoft.EntityFrameworkCore;
using MCC.Data;
using MCC.Models;
using Microsoft.AspNetCore.Identity;
using MCC.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            // Configura��o da conex�o com o banco de dados
            var connString = builder.Configuration.GetConnectionString("StringConnection");
            builder.Services.AddDbContext<UserDBContext>(opts =>
            {
                opts.UseNpgsql(connString);
            });

            // Configura��o do Identity
            builder.Services
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<UserDBContext>()
                .AddDefaultTokenProviders();

            // Configura��o do AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Registro dos servi�os
            builder.Services
                .AddScoped<RegisterService>()
                .AddScoped<LoginService>()
                .AddScoped<TokenService>();

            builder.Services.AddControllers();

            // Configura��o do Swagger
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
            // Configura��o da autentica��o JWT
            var jwtKey = builder.Configuration["Jwt:Key"]; // Pegando do appsettings.json
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)), // Chave do JWT
                };
            });

            // Configura��o da autoriza��o
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequerProfessor", policy =>
                    policy.RequireRole("TEACHER"));

                options.AddPolicy("RequerAluno", policy =>
                    policy.RequireRole("STUDENT"));
            });

            var app = builder.Build();

            // Configura��o do pipeline de requisi��es HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Habilita a autentica��o e autoriza��o
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
