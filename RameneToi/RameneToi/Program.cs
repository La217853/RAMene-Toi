using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
// Ajoutez cette directive using pour les extensions SQL Server :
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using RameneToi.Data;
using RameneToi.Models;

namespace RameneToi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<RameneToiWebAPIContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("RameneToiWebAPIContext") ?? throw new InvalidOperationException("Connection string 'RameneToiWebAPIContext' not found.")));
            
            //service pour hasher mdp et enregistrer le MDP hasher S
            builder.Services.AddScoped<IPasswordHasher<Utilisateurs>, PasswordHasher<Utilisateurs>>();

            // Add services to the container.

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
