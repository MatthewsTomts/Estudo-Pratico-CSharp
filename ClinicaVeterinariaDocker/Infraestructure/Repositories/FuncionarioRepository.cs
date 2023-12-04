using estudo_final.Application.Services;
using estudo_final.Models;
using estudo_final.Models.FuncionarioAggregate;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;
using static estudo_final.Models.FuncionarioAggregate.Funcionario;

namespace estudo_final.Infraestructure.Repositories;

public class FuncionarioRepository : IFuncionarioRepository {
    private readonly Connection _connection = new();
    public void CadastrarFuncionario(Funcionario funcionario) {
        _connection.Funcionario.Add(funcionario);
        _connection.SaveChanges();
    }

    public void DemitirFuncionario(int nif) {
        Funcionario funcionarioDB = _connection.Funcionario.Find(nif);
        if (funcionarioDB != null ) {
            funcionarioDB.Status = (StatusUser)1;

            _connection.Entry(funcionarioDB).State = EntityState.Modified;
            _connection.SaveChanges();
        }
    }

    public void EditarPerfil(Funcionario funcionario) {
        Funcionario funcionarioDB = _connection.Funcionario.Where(f => f.Nif == funcionario.Nif && f.Status == 0)
            .FirstOrDefault();

        if (funcionarioDB != null ) {
            if (funcionario.Senha != null ) {
                funcionarioDB.Senha = funcionario.Senha;
            }

            if (funcionario.Nome != null) {
                funcionarioDB.Nome = funcionario.Nome;
            }

            if (funcionario.Cargo != null) {
                funcionarioDB.Cargo = funcionario.Cargo;
            }

            _connection.Entry(funcionarioDB).State = EntityState.Modified;
            _connection.SaveChanges();
        }
    }

    public Funcionario Login(Funcionario funcionario) {
        Funcionario funcionarioDB = _connection.Funcionario.Where(f => f.Nif == funcionario.Nif && f.Status == 0)
            .FirstOrDefault();

        if (EncryptService.Validar(funcionarioDB.Senha, funcionario.Senha)) {
            return new Funcionario(funcionarioDB.Nif, (CargoFuncionario)funcionarioDB.Cargo);
        }

        return new Funcionario(-1, 0);
    }

    public Funcionario PesquisarFuncionario(int nif) {
        Funcionario funcionario = _connection.Funcionario.Find(nif);
        funcionario.Senha = null;
        return funcionario;
    }
}
