namespace ClinicaVeterinaria.Domain.Models.FuncionarioAggregate;

public interface IFuncionarioRepository {
    public Funcionario Logar(Funcionario funcionario);
    public void CadastrarFuncionario(Funcionario funcionario);
    public void DemitirFuncionario(int nif);
    public void EditarSenha(int nif, string novaSenha);
    public Funcionario PesquisarFuncionario(int nif);
}
