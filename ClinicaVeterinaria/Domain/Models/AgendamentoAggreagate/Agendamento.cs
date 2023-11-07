using ClinicaVeterinaria.Domain.Models.ClienteAggregate;
using ClinicaVeterinaria.Domain.Models.FuncionarioAggregate;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaVeterinaria.Domain.Models.AgendamentoAggreagate;

[Table("agendamentos")]
public class Agendamento {
    [Key]
    public int idAgendamento { get; set; }
    public string? nomeAnimal { get; set; }
    public Especie? especie { get; set; }
    public TimeOnly horario { get; set; }
    public DateOnly data {  get; set; }
    public Status status { get; set; }
    public int? idCliente { get; set; }
    public int nif {  get; set; }

    [ForeignKey(nameof(idCliente))]
    public Cliente Cliente { get; set; }
    [ForeignKey(nameof(nif))]
    public Funcionario funcionario { get; set; }

    public Agendamento() { }

    public Agendamento(DateOnly data, TimeOnly horario, Status status, int? idCliente, int nif)
    {
        this.data = data;
        this.horario = horario;
        this.status = status;
        this.idCliente = idCliente;
        this.nif = nif;
    }

    public enum Especie {
        Gato,
        Cachorro,
        Coelho,
        Roedor,
        Ave
    }

    public enum Status {
        Disponivel,
        Marcado,
        Finalizado,
        Cancelado
    }
}
