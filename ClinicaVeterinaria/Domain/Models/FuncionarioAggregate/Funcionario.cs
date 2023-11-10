using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Domain.Models.FuncionarioAggregate;

[Table("funcionarios")]
public class Funcionario {
    [Key]
    public int nif { get; set; }
    public string nome { get; set; }
    public string senha { get; set; }
    public Cargo cargo { get; set; }
    public Status status { get; set; }

    public Funcionario(int nif, string nome, string senha, Cargo cargo)
    {
        this.nif = nif;
        this.nome = nome;
        this.senha = senha;
        this.cargo = cargo;
    }

    public Funcionario(int nif, string senha, Cargo cargo)
    {
        this.nif = nif;
        this.senha = senha;
        this.cargo = cargo;
    }

    public Funcionario(int nif, string senha)
    {
        this.nif = nif;
        this.senha = senha;
    }

    public Funcionario(int nif, Cargo cargo)
    {
        this.nif = nif;
        this.cargo = cargo;
    }

    private Funcionario() { }

    public enum Cargo {
        Administrador,
        Veterinario
    }

    public enum Status {
        Ativo,
        Inativo
    }
}

