using ClinicaVeterinaria.Domain.Models.ClienteAggregate;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using static ClinicaVeterinaria.Domain.Models.ClienteAggregate.Cliente;

namespace ClinicaVeterinaria.Infraestructure.Repositories;

public class ClienteRepository : IClienteRepository {
    private readonly Conn _connection = new();
    public void Cadastro(Cliente cliente) {
        // Adds the Cliente to the DB

        // Generates a salt
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create()) {
            rng.GetBytes(salt);
        }

        // Hash the password with the salt
        using (var pbkdf2 = new Rfc2898DeriveBytes(cliente.senha, salt, 10000)) {
            byte[] hash = pbkdf2.GetBytes(20); // 20 bytes for a 160-bit key
            byte[] hashBytes = new byte[36];  // 16 bytes salt + 20 bytes hash

            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            cliente.senha = Convert.ToBase64String(hashBytes);
        }

        _connection.Cliente.Add(cliente);
        _connection.SaveChanges();
    }

    public void EditarPerfil(Cliente cliente, int idCliente) {
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

    public int Logar(Cliente cliente) {
        // Search for the username on the DB if found it returns its id, if not returns null
        Cliente clienteDB = _connection.Cliente.Where(c => c.email == cliente.email && c.status == 0)
            .Select(c => new Cliente (
                c.idCliente,
                c.senha
             )).ToList()
            .FirstOrDefault();

        byte[] salt = new byte[16];
        byte[] passwordDB = new byte[36];

        passwordDB = Convert.FromBase64String(clienteDB.senha);

        Array.Copy(passwordDB, 0, salt, 0, 16);

        byte[] passwordLogin = new byte[16];

        using (var pbkdf2 = new Rfc2898DeriveBytes(cliente.senha, salt, 10000))
        {
            byte[] hash = pbkdf2.GetBytes(20); // 20 bytes for a 160-bit key
            byte[] hashBytes = new byte[36];  // 16 bytes salt + 20 bytes hash

            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            passwordLogin = hashBytes;
        }

        if (Convert.ToBase64String(passwordLogin) == Convert.ToBase64String(passwordDB)) {
            return clienteDB.idCliente;
        } else {
            return 0;
        }
    }

    public void ApagarPerfil(int idCliente) {
        Cliente cliente =_connection.Cliente.Where(c => c.idCliente == idCliente).FirstOrDefault();
        cliente.status = (Status)1;

        _connection.Entry(cliente).State = EntityState.Modified;
        _connection.SaveChanges();
    }

    public string PedidoRecuperarSenha(string email) {
        var cliente = _connection.Cliente.Where(c => c.email == email && c.status == 0).FirstOrDefault();

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

    public bool RecuperarSenha(Cliente cliente, string codigoValidacao) {
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
