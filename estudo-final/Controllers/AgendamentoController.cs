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
    public IActionResult CadastrarHorario([FromForm][Required] DateOnly data,[Required] TimeOnly horario) {
        int idFuncionario = TokenService.RecuperarId(HttpContext);

        Agendamento agendamento = new(data, horario, idFuncionario);

        try {
            _agendamentoRepository.CadastrarHorario(agendamento);
            return Ok();
        } catch (Exception ex) {
            Console.WriteLine(ex.ToString());
            return BadRequest();
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
    public IActionResult FinalizarAgendamento([FromForm] int idAgendamento) {
        int idFuncionario = TokenService.RecuperarId(HttpContext);

        _agendamentoRepository.FinalizarAgendamento(idAgendamento, idFuncionario);
        return Ok();
    }

    [HttpPost]
    [Authorize(Policy = "RequireCliente")]
    [Route("cliente/agendar")]
    public IActionResult AgendarConsulta([FromForm][Required] int idAgendamento, [Required] string nomeAnimal,
           [Required] EspecieAnimal especie) {
        int idCliente = TokenService.RecuperarId(HttpContext);

        Agendamento agendamento = new(idAgendamento, nomeAnimal, especie, idCliente);

        try {
            _agendamentoRepository.AgendarConsulta(agendamento);
            return Ok();
        } catch (Exception ex) {
            Console.WriteLine(ex.ToString());
            return BadRequest();
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
    public IActionResult CancelarConsulta([FromForm] int idAgendamento) {
        int idUser = TokenService.RecuperarId(HttpContext);

        _agendamentoRepository.CancelarAgendamento(idAgendamento, idUser);
        return Ok();
    }

    [HttpPatch]
    [Route("pesquisar")]
    public IActionResult PesquisarAgendamento([FromForm] DateOnly data) {
        List<Agendamento> agendamentos = _agendamentoRepository.PesquisarAgendamento(data);
        return Ok(agendamentos);
    }
}
