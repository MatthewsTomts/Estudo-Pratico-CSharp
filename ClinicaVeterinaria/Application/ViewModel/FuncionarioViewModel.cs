using static ClinicaVeterinaria.Domain.Models.FuncionarioAggregate.Funcionario;

namespace ClinicaVeterinaria.Application.ViewModel;

public class FuncionarioViewModel {
    public int nif { get; set; }
    public string nome { get; set; }
    public string senha { get; set; }
    public Cargo cargo { get; set; }
}
