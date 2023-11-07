using ClinicaVeterinaria.Domain.Models.AgendamentoAggreagate;
using ClinicaVeterinaria.Domain.Models.ClienteAggregate;
using ClinicaVeterinaria.Domain.Models.FuncionarioAggregate;
using Microsoft.EntityFrameworkCore;
using static ClinicaVeterinaria.Domain.Models.FuncionarioAggregate.Funcionario;

namespace ClinicaVeterinaria.Infraestructure;

public class Conn : DbContext
{
    // This line is used to map the class to the DB
    public DbSet<Cliente> Cliente { get; set; }
    public DbSet<Funcionario> Funcionario { get; set; }
    public DbSet<Agendamento> Agendamento { get; set; }

    // Creates the Connection with the PostgreSQL
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql(
            "Server=localhost;" +
            "Port=5432;" +
            "Database=ClinicaVeterinaria;" +
            "User Id=postgres;" +
            "Password=Senai115@;");
}
