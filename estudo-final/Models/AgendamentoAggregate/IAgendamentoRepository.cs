using static estudo_final.Models.AgendamentoAggregate.Agendamento;

namespace estudo_final.Models.AgendamentoAggregate;

public interface IAgendamentoRepository {
    public List<Agendamento> ListarAgendamentos(int nif);
    public void CadastrarHorario(Agendamento agendamento);
    public List<Agendamento> ListarConsultas(int idCliente);
    public void AgendarConsulta(Agendamento agendamento);
    public List<Agendamento> PesquisarAgendamento(DateOnly data);
    public void CancelarAgendamento(int idAgendamento, int idUser);
    public void FinalizarAgendamento(int idAgendamento, int idFuncionario);
}
