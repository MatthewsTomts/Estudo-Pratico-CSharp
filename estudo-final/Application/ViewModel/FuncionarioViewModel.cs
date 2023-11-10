using static estudo_final.Models.FuncionarioAggregate.Funcionario;

namespace estudo_final.Application.ViewModel;

public class FuncionarioViewModel {
    public int nif { get; set; }
    public string nome { get; set; }
    public string senha { get; set; }
    public CargoFuncionario cargo { get; set; }
}
