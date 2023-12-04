using estudo_final.Application.Services;
using estudo_final.Models.AgendamentoAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static estudo_final.Models.AgendamentoAggregate.Agendamento;

namespace estudo_final.Controllers;

[ApiController]
[Route("/api/agendamento")]
public class AgendamentoController : Controller {
    private readonly IAgendamentoRepository _agendamentoRepository;

    public AgendamentoController(IAgendamentoRepository agendamentoRepository) {
        _agendamentoRepository = agendamentoRepository;
    }

    [HttpPost]
    [Authorize(Policy = "RequireVeterinario")]
    [Route("funcionario/cadastro")]
    public IActionResult CadastrarHorario([Required] DateOnly data,[Required] TimeOnly horario) {
        int idFuncionario = TokenService.RecuperarId(HttpContext);

        Agendamento agendamento = new(data, horario, idFuncionario);

        try {
            _agendamentoRepository.CadastrarHorario(agendamento);
            return Ok(new { message = "Horario Cadastrado!" });
        } catch (Exception ex) {
            Console.WriteLine(ex.ToString());
            return BadRequest(new { message = "Um erro ocorreu" });
        }
    }

    [HttpPost]
    [Authorize(Policy = "RequireVeterinario")]
    [Route("funcionario/listar")]
    public IActionResult ListarAgendamentos() {
        int idFuncionario = TokenService.RecuperarId(HttpContext);

        var agendamentos = _agendamentoRepository.ListarAgendamentos(idFuncionario);
        return Ok(agendamentos);
    }

    [HttpPatch]
    [Authorize(Policy = "RequireVeterinario")]
    [Route("funcionario/finalizar")]
    public IActionResult FinalizarAgendamento([Required]int idAgendamento) {
        int idFuncionario = TokenService.RecuperarId(HttpContext);

        _agendamentoRepository.FinalizarAgendamento(idAgendamento, idFuncionario);
        return Ok(new { message = "Agendamento Finalizado" });
    }

    [HttpPost]
    [Authorize(Policy = "RequireCliente")]
    [Route("cliente/agendar")]
    public IActionResult AgendarConsulta([Required] int idAgendamento, [Required] string nomeAnimal,
           [Required] EspecieAnimal especie) {
        int idCliente = TokenService.RecuperarId(HttpContext);

        Agendamento agendamento = new(idAgendamento, nomeAnimal, especie, idCliente);

        try {
            _agendamentoRepository.AgendarConsulta(agendamento);
            return Ok(new { message = "Consulta Agendada" });
        } catch (Exception ex) {
            Console.WriteLine(ex.ToString());
            return BadRequest(new { message = "Um erro ocorreu" });
        }
    }

    [HttpPost]
    [Authorize(Policy = "RequireCliente")]
    [Route("cliente/listar")]
    public IActionResult ListarConsultas() {
        int idCliente = TokenService.RecuperarId(HttpContext);

        var agendamentos = _agendamentoRepository.ListarConsultas(idCliente);
        return Ok(agendamentos);
    }

    [HttpDelete]
    [Route("cancelar")]
    public IActionResult CancelarConsulta([Required]int idAgendamento) {
        int idUser = TokenService.RecuperarId(HttpContext);

        _agendamentoRepository.CancelarAgendamento(idAgendamento, idUser);
        return Ok(new { message = "Agendamento cancelado" });
    }

    [HttpPatch]
    [Route("pesquisar")]
    public IActionResult PesquisarAgendamento([Required]DateOnly data) {
        List<Agendamento> agendamentos = _agendamentoRepository.PesquisarAgendamento(data);
        return Ok(agendamentos);
    }

    [HttpGet]
    [Route("listar")]
    public IActionResult ListarAgendamento()
    {
        List<Agendamento> agendamentos = _agendamentoRepository.ListarAgendamento();
        return Ok(agendamentos);
    }
}
