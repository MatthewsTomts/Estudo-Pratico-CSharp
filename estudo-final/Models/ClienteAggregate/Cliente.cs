using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace estudo_final.Models.ClienteAggregate;

[Table("clientes")]
public class Cliente
{
    private int idCliente;

    [Key]
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Senha { get; set; }
    public StatusUser Status { get; set; }

    Cliente() { }

    public Cliente(string nome, string email, string senha) {
        Nome = nome;
        Email = email;
        Senha = senha;
    }

    public Cliente(string email, string senha)
    {
        Email = email;
        Senha = senha;
    }

    public Cliente(int idCliente, string email, string nome)
    {
        Id = idCliente;
        Email = email;
        Nome = nome;
    }

    public Cliente(int idCliente, string senha)
    {
        Id = idCliente;
        Senha = senha;
    }
}
