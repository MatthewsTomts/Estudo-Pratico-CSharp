using ClinicaVeterinaria.Domain.Models.FuncionarioAggregate;

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

    public Funcionario Logar(Funcionario funcionario) {
        var result = _connection.Funcionario.Where(c =>
            c.nif == funcionario.nif && c.senha == funcionario.senha)
            .Select(c => new {
                Nif = c.nif,
                Cargo = c.cargo
            })
            .FirstOrDefault();

        Funcionario funcionarioDB = new(result.Nif, result.Cargo);

        return funcionarioDB;
    }
}
