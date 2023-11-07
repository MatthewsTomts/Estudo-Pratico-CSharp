using ClinicaVeterinaria.Domain.Models.FuncionarioAggregate;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Infraestructure.Repositories;

public class FuncionarioRepository : IFuncionarioRepository {
    private Conn _connection = new();
    public void CadastrarFuncionario(Funcionario funcionario)
    {
        _connection.Funcionario.Add(funcionario);
        _connection.SaveChanges();
    }

    public void DemitirFuncionario(int nif)
    {
        var funcionarioDB = _connection.Funcionario.Find(nif);
        if (funcionarioDB != null) {
            _connection.Funcionario.Remove(funcionarioDB);
            _connection.SaveChanges();
        }
    }

    public void EditarSenha(int nif, string novaSenha)
    {
        var funcionario = _connection.Funcionario.Find(nif);

        if (funcionario != null ) { 
            funcionario.senha = novaSenha;

            _connection.Entry(funcionario).State = EntityState.Modified;

            _connection.SaveChanges();
        }
    }

    public Funcionario Logar(Funcionario funcionario) {
        var result = _connection.Funcionario.Where(c =>
            c.nif == funcionario.nif && c.senha == funcionario.senha)
            .Select(c => new {
                Nif = c.nif,
                Cargo = c.cargo
            })
            .FirstOrDefault();

        if (result == null)
        {
            return null;
        } else
        {
            return new Funcionario(result.Nif, result.Cargo);
        }
    }

    public Funcionario PesquisarFuncionario(int nif)
    {
        return _connection.Funcionario.Find(nif);
    }
}
