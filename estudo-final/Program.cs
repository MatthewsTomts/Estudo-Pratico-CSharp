using Microsoft.AspNetCore.Authentication.JwtBearer;
using estudo_final.Infraestructure.Repositories;
using estudo_final.Models.ClienteAggregate;
using estudo_final.Application.Swagger;
using Microsoft.IdentityModel.Tokens;
using estudo_final;
using System.Text;
using Microsoft.OpenApi.Models;
using estudo_final.Models.FuncionarioAggregate;
using estudo_final.Models.AgendamentoAggregate;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>{
    c.OperationFilter<SwaggerDefaultValues>();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement() { {
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header
        },
        new List<string>()
    }});
});

// Cria a comunicação entre as interfaces e as classes dos repositorios
builder.Services.AddTransient<IClienteRepository, ClienteRepository>();
builder.Services.AddTransient<IFuncionarioRepository, FuncionarioRepository>();
builder.Services.AddTransient<IAgendamentoRepository, AgendamentoRepository>();

// Adiciona o CORS
builder.Services.AddCors(options => {
    options.AddPolicy(name: "myPolicy", policy => {
        policy.AllowAnyOrigin() // Permite qualquer Origem
            .AllowAnyHeader()  // Permite qualquer Header
            .AllowAnyMethod(); // Permite qualquer Método
    });
});

// Encodifica a chave
var key = Encoding.ASCII.GetBytes(SecretKey.Key);

// Criando autenticação
builder.Services.AddAuthentication(x => {
    // Define o metodo de autenticação
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // Define o metodo de Challenge
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    // Pede os metadados do HTTPS
    x.RequireHttpsMetadata = true;
    // Armazena o token
    x.SaveToken = true;
    // Estabelece os metodos de Validação do Token
    x.TokenValidationParameters = new TokenValidationParameters {
        // Valida a chave do criador
        ValidateIssuerSigningKey = true,
        // Chave do criador
        IssuerSigningKey = new SymmetricSecurityKey(key),
        // Não valida o criador
        ValidateIssuer = false,
        // Não valida o receptor
        ValidateAudience = false 
    };
});

// Cria os níveis de autorização
builder.Services.AddAuthorization(options => {
    // Cria uma autorização para o administrador
    options.AddPolicy("RequireAdmin", policy => {
        policy.RequireClaim("tipo", "Administrador");
    });

    // Cria uma autorização para o Veterinario
    options.AddPolicy("RequireVeterinario", policy => {
        policy.RequireClaim("tipo", "Veterinario");
    });

    // Cria uma autorização para o Cliente
    options.AddPolicy("RequireCliente", policy => {
        policy.RequireClaim("tipo", "Cliente");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("myPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
