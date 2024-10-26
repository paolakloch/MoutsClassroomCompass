using Microsoft.EntityFrameworkCore;
using MCC.Data;
using MCC.Models;
using Microsoft.AspNetCore.Identity;
using MCC.Services;

namespace MCC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            var connString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<UserDBContext>(opts =>
            {
                opts.UseNpgsql(connString);
            });

            builder.Services
                .AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<UserDBContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAutoMapper
                  (AppDomain.CurrentDomain.GetAssemblies());


            builder.Services
                .AddScoped<RegisterService>() // optei por usar scoped - o servico vai sempre ser instanciado quando houver uma requisição nova que demande uma instancia desse register service // o singleton seria 1 unico cadastro service pra todas as reqs seria a mesma instancia // add transient faz sempre uma instancia nova mesmo que na mesma req
                .AddScoped<LoginService>()
                .AddScoped<TokenService>();

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

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
