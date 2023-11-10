using ClinicaVeterinaria.Domain.Models.AgendamentoAggreagate;
using ClinicaVeterinaria.Domain.Models.FuncionarioAggregate;
using ClinicaVeterinaria.Domain.Models.ClienteAggregate;
using ClinicaVeterinaria.Infraestructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ClinicaVeterinaria.Application.Swagger;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ClinicaVeterinaria.Domain.Models.FuncionarioAggregate;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add Controllers to the API
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<SwaggerDefaultValues>();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        },
    });
});

// Connect the Interface to the Class
builder.Services.AddTransient<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddTransient<IClienteRepository, ClienteRepository>();
builder.Services.AddTransient<IAgendamentoRepository, AgendamentoRepository>();

// Creates the Cors Policy
builder.Services.AddCors(options => {
    options.AddPolicy(name: "myPolicy",
        policy => {
            policy.WithOrigins("https://localhost:8080", "http://localhost:8080")
            .AllowAnyHeader().AllowAnyMethod();
        });
});

// Encode the secret key
var key = Encoding.ASCII.GetBytes(ClinicaVeterinaria.ApiKey.Secret);

// Setup the authentication method
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true; // Store the token after create it
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // It validates the signing key
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false, // It doesn't validate the creator of the token
        ValidateAudience = false // It doesn't validate the recipient of the token
    };
});

builder.Services.AddAuthorization(options => {
    options.AddPolicy("RequireAdmin", policy =>
    {
        policy.RequireClaim("tipo", "Administrador");
    });

    options.AddPolicy("RequireCliente", policy =>
    {
        policy.RequireClaim("tipo", "Cliente");
    });

    options.AddPolicy("RequireVeterinario", policy =>
    {
        policy.RequireClaim("tipo", "Veterinario");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("myPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
