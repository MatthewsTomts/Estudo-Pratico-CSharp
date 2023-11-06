namespace ClinicaVeterinaria.Domain.Models.FuncionarioAggregate;

public interface IFuncionarioRepository {
    public Funcionario Logar(Funcionario funcionario);
    public void CadastrarFuncionario(Funcionario funcionario);
    public void DemitirFuncionario(int nif);
}
