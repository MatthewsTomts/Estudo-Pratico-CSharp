using ClinicaVeterinaria.Domain.Models.FuncionarioAggregate;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using static ClinicaVeterinaria.Domain.Models.FuncionarioAggregate.Funcionario;

namespace ClinicaVeterinaria.Infraestructure.Repositories;

public class FuncionarioRepository : IFuncionarioRepository {
    private Conn _connection = new();
    public void CadastrarFuncionario(Funcionario funcionario) {
        byte[] salt = new byte[16];
        using (var rng = RandomNumberGenerator.Create()) {
            rng.GetBytes(salt);
        }

        using (var pkbdf2 = new Rfc2898DeriveBytes(funcionario.senha, salt, 10000)) {
            byte[] hash = pkbdf2.GetBytes(20);
            byte[] hashPass = new byte[36];

            Array.Copy(salt, 0, hashPass, 0, 16);
            Array.Copy(hash, 0, hashPass, 16, 20);

            funcionario.senha = Convert.ToBase64String(hashPass);
        }

        _connection.Funcionario.Add(funcionario);
        _connection.SaveChanges();
    }

    public void DemitirFuncionario(int nif) {
        var funcionarioDB = _connection.Funcionario.Find(nif);
        if (funcionarioDB != null) {
            _connection.Funcionario.Remove(funcionarioDB);
            _connection.SaveChanges();
        }
    }

    public void EditarSenha(int nif, string novaSenha) {
        var funcionario = _connection.Funcionario.Find(nif);

        if (funcionario != null ) { 
            funcionario.senha = novaSenha;

            _connection.Entry(funcionario).State = EntityState.Modified;

            _connection.SaveChanges();
        }
    }

    public Funcionario Logar(Funcionario funcionario) {
        Funcionario funcionarioDB = _connection.Funcionario.Where(c =>
            c.nif == funcionario.nif && c.status == 0)
            .Select(c => new Funcionario(
                c.nif,
                c.senha,
                c.cargo
            ))
            .FirstOrDefault();

        if (funcionarioDB == null) {
            return null;
        } else {
            byte[] salt = new byte[16];
            byte[] passwordDB = new byte[36];
            passwordDB = Convert.FromBase64String(funcionarioDB.senha);

            Array.Copy(passwordDB, 0, salt, 0, 16);

            byte[] passwordFun = new byte[36];
            using (var hash = new Rfc2898DeriveBytes(funcionario.senha, salt, 10000)) {
                byte[] hashByte = hash.GetBytes(20);
                byte[] hashByteFull = new byte[36];

                Array.Copy(salt, 0, hashByteFull, 0, 16);
                Array.Copy(hashByte, 0, hashByteFull, 16, 20);

                passwordFun = hashByteFull;
            }

            if (Convert.ToBase64String(passwordFun) == Convert.ToBase64String(passwordDB)) {
                return new Funcionario(funcionarioDB.nif, funcionarioDB.cargo);
            } else {
                return null;
            }
        }
    }

    public Funcionario PesquisarFuncionario(int nif)
    {
        return _connection.Funcionario.Find(nif);
    }
}
