using ClinicaVeterinaria.Domain.Models.ClienteAggregate;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Infraestructure.Repositories;

public class ClienteRepository : IClienteRepository {
    private readonly Conn _connection = new();
    public void Cadastro(Cliente cliente)
    {
        // Adds the Cliente to the DB
        _connection.Cliente.Add(cliente);
        _connection.SaveChanges();
    }

    public void EditarPerfil(Cliente cliente, int idCliente)
    {
        var clienteDB = _connection.Cliente.Find(idCliente);

        if (cliente.nome != null) {
            clienteDB.nome = cliente.nome;
        }

        if (cliente.email != null)
        {
            clienteDB.email = cliente.email;
        }

        if (cliente.senha != null)
        {
            clienteDB.senha = cliente.senha;
        }

        _connection.Entry(clienteDB).State = EntityState.Modified;

        _connection.SaveChanges();
    }

    public int Logar(Cliente cliente)
    {
        // Search for the username on the DB if found it returns its id, if not returns null
        return _connection.Cliente.Where(c => c.email == cliente.email && c.senha == cliente.senha)
            .Select(c => c.idCliente)
            .FirstOrDefault();
    }

    public string PedidoRecuperarSenha(string email)
    {
        var cliente = _connection.Cliente.Where(c => c.email == email).FirstOrDefault();

        if (cliente != null) {
            Random random = new Random();
            string randomNumber = random.Next(100000, 999999).ToString();

            cliente.senha = randomNumber;

            _connection.Entry(cliente).State = EntityState.Modified;

            _connection.SaveChanges();

            return randomNumber;
        }

        return "-1";
    }

    public bool RecuperarSenha(Cliente cliente, string codigoValidacao)
    {
        var clienteDB = _connection.Cliente.Where(c => c.email == cliente.email && c.senha == codigoValidacao)
            .FirstOrDefault();

        if (clienteDB != null)
        {
            clienteDB.senha = cliente.senha;

            _connection.Entry(clienteDB).State = EntityState.Modified;

            _connection.SaveChanges();

            return true;
        }

        return false;
    }
}
