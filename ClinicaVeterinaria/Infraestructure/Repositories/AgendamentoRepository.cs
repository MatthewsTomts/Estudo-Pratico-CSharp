using ClinicaVeterinaria.Domain.Models.AgendamentoAggreagate;
using ClinicaVeterinaria.Domain.Models.ClienteAggregate;
using Microsoft.EntityFrameworkCore;
using static ClinicaVeterinaria.Domain.Models.AgendamentoAggreagate.Agendamento;

namespace ClinicaVeterinaria.Infraestructure.Repositories;

public class AgendamentoRepository : IAgendamentoRepository
{
    private readonly Conn _connection = new();

    // Veterinario
    public List<Agendamento> ListarAgendamentos(int nif) => _connection.Agendamento
        .Where(agendamento => agendamento.nif == nif).ToList();

    public void CadastrarHorario(DateOnly data, TimeOnly horario, int nif)
    {
        Agendamento agendamento = new(data, horario, 0, null, nif);

        _connection.Agendamento.Add(agendamento);
        _connection.SaveChanges();
    }

    // Cliente
    public List<Agendamento> ListarConsultas(int idCliente) => _connection.Agendamento
        .Where(agendamento => agendamento.idCliente == idCliente).ToList();

    public void AgendarConsulta(int idAgendamento, Especie especie, string nomeAnimal, int idCliente)
    {
        Agendamento agendamento = _connection.Agendamento.Where(agendamento => agendamento.idAgendamento == idAgendamento)
            .FirstOrDefault();

        agendamento.status = (Status)1;
        agendamento.especie = especie;
        agendamento.nomeAnimal = nomeAnimal;
        agendamento.idCliente = idCliente;

        _connection.Entry(agendamento).State = EntityState.Modified;
        _connection.SaveChanges();
    }

    // Ambos
    public List<Agendamento> PesquisarAgendamentos(DateOnly data) => _connection.Agendamento
        .Where(agendamento => agendamento.data == data).ToList();

    public void CancelarConsulta(int idAgendamento)
    {
        Agendamento agendamento = _connection.Agendamento.Where(a => a.idAgendamento == idAgendamento).FirstOrDefault();

        agendamento.status = (Status)4;

        _connection.Entry(agendamento).State = EntityState.Modified;
        _connection.SaveChanges();
    }

    public void FinalizarConsulta(int idAgendamento)
    {
        Agendamento agendamento = _connection.Agendamento.Where(a => a.idAgendamento == idAgendamento).FirstOrDefault();

        agendamento.status = (Status)3;

        _connection.Entry(agendamento).State = EntityState.Modified;
        _connection.SaveChanges();
    }
}
