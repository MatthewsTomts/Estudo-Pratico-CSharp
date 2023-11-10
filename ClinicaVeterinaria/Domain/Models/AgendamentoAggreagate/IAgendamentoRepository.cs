using static ClinicaVeterinaria.Domain.Models.AgendamentoAggreagate.Agendamento;

namespace ClinicaVeterinaria.Domain.Models.AgendamentoAggreagate;

public interface IAgendamentoRepository {
    public List<Agendamento> ListarAgendamentos(int nif);
    public void CadastrarHorario(DateOnly data, TimeOnly horario, int nif);
    public List<Agendamento> ListarConsultas(int idCliente);
    public void AgendarConsulta(int idAgendamento, Especie especie, string nomeAnimal, int idCliente);
    public List<Agendamento> PesquisarAgendamentos(DateOnly data);
    public void CancelarConsulta(int idAgendamento);
    public void FinalizarConsulta(int idAgendamento);
}
