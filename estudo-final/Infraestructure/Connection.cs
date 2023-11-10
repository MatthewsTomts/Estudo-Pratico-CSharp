using estudo_final.Models.AgendamentoAggregate;
using estudo_final.Models.FuncionarioAggregate;
using estudo_final.Models.ClienteAggregate;
using Microsoft.EntityFrameworkCore;

namespace estudo_final.Infraestructure;

public class Connection : DbContext {
    // Cria uma representação da tabela clientes
    public DbSet<Cliente> Cliente { get; set; }
    // Cria uma representação da tabela funcionarios
    public DbSet<Funcionario> Funcionario { get; set; }
    // Cria uma representação da tabela agendamentos
    public DbSet<Agendamento> Agendamento { get; set; }

    // Configura a conexão com o banco Postgres
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql(
            "Server=localhost;" +
            "Port=5432;" +
            "User Id=postgres;" +
            "Password=Senai115@;" +
            "Database=EstudoFinal;");
}
