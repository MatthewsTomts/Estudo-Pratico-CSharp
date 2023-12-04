using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace estudo_final.Models.FuncionarioAggregate;

[Table("funcionarios")]
public class Funcionario
{
    [Key]
    public int Nif { get; set; }
    public string Nome { get; set; }
    public string? Senha { get; set; }
    public CargoFuncionario Cargo { get; set; }
    public StatusUser Status { get; set; }

    Funcionario() { }

    public Funcionario(int nif, string nome, string senha, CargoFuncionario cargo, StatusUser statusUser) {
        Nif = nif;
        Nome = nome;
        Senha = senha;
        Cargo = cargo;
        Status = statusUser;
    }

    public Funcionario(int nif, string senha) {
        Nif = nif;
        Senha = senha;
    }

    public Funcionario(int nif, CargoFuncionario cargo) {
        Nif = nif;
        Cargo = cargo;
    }

    public Funcionario(int nif, string nome, string senha, CargoFuncionario cargo) {
        Nif = nif;
        Nome = nome;
        Senha = senha;
        Cargo = cargo;
    }

    public enum CargoFuncionario {
        Administrador = 0,
        Veterinario = 1
    }
}
