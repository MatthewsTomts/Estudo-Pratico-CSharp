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

    // Used to create the Cliente
    public Cliente (string nome, string email, string senha)
    {
        this.nome = nome;
        this.email = email;
        this.senha = senha;
    }

    // Used to Login
    public Cliente(string email, string senha)
    {
        this.email = email;
        this.senha = senha;
    }

    // Used to generate tokens
    public Cliente (int idCliente)
    {
        this.idCliente = idCliente;
    }
}
