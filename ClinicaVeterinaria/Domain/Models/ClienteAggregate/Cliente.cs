using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.Domain.Models.ClienteAggregate;

[Table("clientes")]
public class Cliente
{
    [Key]
    public int idCliente { get; set; }
    public string nome { get; set; }
    public string email { get; set; }
    public string senha { get; set; }
    public Status status { get; set; }

    // Used to create the Cliente
    // Used to alter the Cliente
    public Cliente (string? nome, string? email, string? senha)
    {
        this.nome = nome;
        this.email = email;
        this.senha = senha;
    }

    // Used to Login
    // Used to Recover Password
    public Cliente(string email, string senha)
    {
        this.email = email;
        this.senha = senha;
    }

    public Cliente(int idCliente, string senha)
    {
        this.idCliente = idCliente;
        this.senha = senha;
    }

    public Cliente() { }

    public enum Status {
        Ativo,
        Inativo
    }
}
