using estudo_final.Models.AgendamentoAggregate;
using estudo_final.Models.FuncionarioAggregate;
using Microsoft.EntityFrameworkCore;
using static estudo_final.Models.AgendamentoAggregate.Agendamento;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace estudo_final.Infraestructure.Repositories;

public class AgendamentoRepository : IAgendamentoRepository {
    private readonly Connection _connection = new();

    public void AgendarConsulta(Agendamento agendamento) {
        Agendamento agendamentoDB = _connection.Agendamento.Where(a => a.IdAgendamento == agendamento.IdAgendamento
            && a.Status == 0).FirstOrDefault();

        if (agendamentoDB != null) {
            agendamentoDB.IdCliente = agendamento.IdCliente;
            agendamentoDB.NomeAnimal = agendamento.NomeAnimal;
            agendamentoDB.Especie = agendamento.Especie;
            agendamentoDB.Status = (StatusAgendamento)1;

            _connection.Entry(agendamentoDB).State = EntityState.Modified;
            _connection.SaveChanges();
        }
    }

    public void CadastrarHorario(Agendamento agendamento) {
        _connection.Agendamento.Add(agendamento);
        _connection.SaveChanges();
    }

    public void CancelarAgendamento(int idAgendamento, int idUser) {
        Agendamento agendamentoDb = _connection.Agendamento.Where(a => a.IdAgendamento == idAgendamento &&
            (a.IdFuncionario == idUser || a.IdCliente == idUser)
            && (a.Status == 0 || a.Status == (StatusAgendamento)1)).FirstOrDefault();

        if (agendamentoDb != null) {
            agendamentoDb.Status = (StatusAgendamento)3;
            _connection.Entry(agendamentoDb).State = EntityState.Modified;
            _connection.SaveChanges();
        }
    }

    public void FinalizarAgendamento(int idAgendamento, int idFuncionario) {
        Agendamento agendamentoDb = _connection.Agendamento.Where(a => a.IdAgendamento == idAgendamento &&
            a.IdFuncionario == idFuncionario && a.Status == (StatusAgendamento)1).FirstOrDefault();

        if (agendamentoDb != null) {
            agendamentoDb.Status = (StatusAgendamento)2;
            _connection.Entry(agendamentoDb).State = EntityState.Modified;
            _connection.SaveChanges();
        }
    }

    public List<Agendamento> ListarAgendamento()
    {
        return _connection.Agendamento.ToList();
    }

    public List<Agendamento> ListarAgendamentos(int nif) {
        return _connection.Agendamento.Where(a => a.IdFuncionario == nif).ToList();
    }

    public List<Agendamento> ListarConsultas(int idCliente) {
        return _connection.Agendamento.Where(a => a.IdCliente == idCliente).ToList();
    }

    public List<Agendamento> PesquisarAgendamento(DateOnly data) {
        return _connection.Agendamento.Where(a => a.Data == data).ToList();
    }
}
