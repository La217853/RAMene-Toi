using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RameneToi.Data;
using RameneToi.Models;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RameneToiWebAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RameneToiWebAPIContext") ?? throw new InvalidOperationException("Connection string 'HELHaExampleWebAPIContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
//service pour hasher mdp et enregistrer le MDP hasher
builder.Services.AddScoped<IPasswordHasher<Utilisateurs>, PasswordHasher<Utilisateurs>>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid BearerToken",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
}
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
