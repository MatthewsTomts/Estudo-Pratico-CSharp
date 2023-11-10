namespace estudo_final.Models.FuncionarioAggregate;

public interface IFuncionarioRepository {
    public Funcionario Login(Funcionario funcionario);
    public void CadastrarFuncionario(Funcionario funcionario);
    public void DemitirFuncionario(int nif);
    public void EditarPerfil(Funcionario funcionario);
    public Funcionario PesquisarFuncionario(int nif);
}
