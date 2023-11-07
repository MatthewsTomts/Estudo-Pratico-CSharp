using ClinicaVeterinaria.Domain.Models.AgendamentoAggreagate;

namespace ClinicaVeterinaria.Infraestructure.Repositories;

public class AgendamentoRepository : IAgendamentoRepository
{
    private readonly Conn _connection = new();

    // Veterinario
    public List<Agendamento> ListarAgendamentos(int nif) => _connection.Agendamento
        .Where(agendamento => agendamento.nif == nif).ToList();

    public void CadastrarHorario(DateOnly data, TimeOnly horario, int nif)
    {
        throw new NotImplementedException();
    }

    // Cliente
    public List<Agendamento> ListarConsultas(int idCliente)
    {
        throw new NotImplementedException();
    }

    public void AgendarConsulta(int idAgendamento, Agendamento.Especie especie, string nomeAnimal, int idCliente)
    {
        throw new NotImplementedException();
    }

    // Ambos
    public List<Agendamento> PesquisarAgendamentos(DateOnly data)
    {
        throw new NotImplementedException();
    }

    public void CancelarConsulta(int idAgendamento)
    {
        throw new NotImplementedException();
    }

    public void FinalizarConsulta(int idAgendamento)
    {
        throw new NotImplementedException();
    }
}
