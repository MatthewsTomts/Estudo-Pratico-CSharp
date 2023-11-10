using estudo_final.Application.Services;
using estudo_final.Models;
using estudo_final.Models.ClienteAggregate;
using Microsoft.EntityFrameworkCore;

namespace estudo_final.Infraestructure.Repositories;

public class ClienteRepository : IClienteRepository {
    private Connection _connection = new();

    public int Logar(Cliente cliente) {
        // Procura o cliente a partir do email
        Cliente clienteDB = _connection.Cliente.Where(cli => cli.Email == cliente.Email 
            && cli.Status == 0).FirstOrDefault();

        // Verifica se o retorno do DB não foi nulo
        if (clienteDB == null) {
            return -1;
        }

        // Verifica se a senha está correta
        if (EncryptService.Validar(clienteDB.Senha, cliente.Senha)) {
            // Se estiver retorna o ID para criar o JWT
            return clienteDB.Id;
        }
        else {
            return -1;
        }
    }

    public void Cadastro(Cliente cliente) {
        // Cadastra o cliente no banco de dados
        _connection.Cliente.Add(cliente);
        _connection.SaveChanges();
    }

    public void ApagarPerfil(int idCliente) {
        Cliente clienteDb = _connection.Cliente.Find(idCliente);
        clienteDb.Status = (StatusUser)1;
        clienteDb.Email = clienteDb.Id.ToString();

        _connection.Entry(clienteDb).State = EntityState.Modified;
        _connection.SaveChanges();
    }

    public void EditarPerfil(Cliente cliente) {
        Cliente clienteDb = _connection.Cliente.Find(cliente.Id);

        if (cliente.Senha != null) {
            clienteDb.Senha = cliente.Senha;
        }

        if (cliente.Email != null) {
            clienteDb.Email = cliente.Email;
        }

        if (cliente.Nome != null) {
            clienteDb.Nome = cliente.Nome;
        }

        _connection.Entry(clienteDb).State = EntityState.Modified;
        _connection.SaveChanges();
    }
    
    public string PedidoRecuperarSenha(string email) {
        throw new NotImplementedException();
    }

    public void RecuperarSenha(string email, string senha, string codigoValidador) {
        throw new NotImplementedException();
    }
}
