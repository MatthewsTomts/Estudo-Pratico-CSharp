using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace estudo_final.Models.AgendamentoAggregate;

[Table("agendamentos")]
public class Agendamento
{
    [Key]
    public int IdAgendamento { get; set; }
    public DateOnly Data { get; set; }
    public TimeOnly Horario { get; set; }
    public string? NomeAnimal { get; set; }
    public EspecieAnimal? Especie { get; set; }
    public StatusAgendamento Status { get; set; }
    [ForeignKey("clientes")]
    public int IdCliente { get; set; }
    [ForeignKey("funcionarios")]
    public int IdFuncionario { get; set; }

    Agendamento() { }

    public Agendamento(DateOnly data, TimeOnly horario, int idFuncionario)
    {
        Data = data;
        Horario = horario;
        IdFuncionario = idFuncionario;
    }

    public Agendamento(int idAgendamento, string nomeAnimal, EspecieAnimal especie, int idCliente)
    {
        IdAgendamento = idAgendamento;
        NomeAnimal = nomeAnimal;
        Especie = especie;
        IdCliente = idCliente;
    }

    public enum EspecieAnimal {
        Gato,
        Cachorro,
        Coelho,
        Ave,
        Roedor
    }

    public enum StatusAgendamento {
        Disponivel,
        Marcado,
        Finalizado,
        Cancelado
    }
}
