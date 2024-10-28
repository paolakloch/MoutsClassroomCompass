using Microsoft.EntityFrameworkCore;
using MCC.Data;
using MCC.Models;
using Microsoft.AspNetCore.Identity;
using MCC.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MCC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuração da conexão com o banco de dados
            var connString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<UserDBContext>(opts =>
            {
                opts.UseNpgsql(connString);
            });

            // Configuração do Identity
            builder.Services
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<UserDBContext>()
                .AddDefaultTokenProviders();

            // Configuração do AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Registro dos serviços
            builder.Services
                .AddScoped<RegisterService>()
                .AddScoped<LoginService>()
                .AddScoped<TokenService>();

            builder.Services.AddControllers();

            // Configuração do Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configuração da autenticação JWT
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

            // Configuração da autorização
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequerProfessor", policy =>
                    policy.RequireRole("TEACHER"));

                options.AddPolicy("RequerAluno", policy =>
                    policy.RequireRole("STUDENT"));
            });

            var app = builder.Build();

            // Configuração do pipeline de requisições HTTP
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Habilita a autenticação e autorização
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
