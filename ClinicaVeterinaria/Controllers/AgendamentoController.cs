using static ClinicaVeterinaria.Domain.Models.AgendamentoAggreagate.Agendamento;
using ClinicaVeterinaria.Domain.Models.AgendamentoAggreagate;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicaVeterinaria.Controllers;

[ApiController]
[Route("/api/agendamento")]
public class AgendamentoController : Controller {
    public IAgendamentoRepository _agendamentoRepository;

    public AgendamentoController(IAgendamentoRepository agendamentoRepository) {
        _agendamentoRepository = agendamentoRepository;
    }

    // Endpoints of the Veterinario
    [Authorize(Policy = "RequireVeterinario")]
    [HttpGet]
    public IActionResult ListarAgendamentos() {
        string jwt = HttpContext.Request.Headers["Authorization"];
        int nif = 0;

        if (!string.IsNullOrEmpty(jwt))  {
            jwt = jwt.Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            if(token.Claims.FirstOrDefault(claim => claim.Type == "userId") is Claim conteudoClaim) {
                nif = int.Parse(conteudoClaim.Value);
            }
        }

        var agendamentos = _agendamentoRepository.ListarAgendamentos(nif);

        return Ok(agendamentos);
    }

    [Authorize(Policy = "RequireVeterinario")]
    [HttpPost]
    public IActionResult CadastrarHorario(DateOnly data, TimeOnly horario)
    {
        string jwt = HttpContext.Request.Headers["Authorization"];
        int nif = 0;

        if (!string.IsNullOrEmpty(jwt))
        {
            jwt = jwt.Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            if (token.Claims.FirstOrDefault(claim => claim.Type == "userId") is Claim conteudoClaim)
            {
                nif = int.Parse(conteudoClaim.Value);
            }
        }

        _agendamentoRepository.CadastrarHorario(data, horario, nif);

        return Ok();
    }


    // Endpoints of the Cliente
    [Authorize(Policy = "RequireCliente")]
    [HttpGet]
    [Route("listarConsultas")]
    public IActionResult ListarConsultas()
    {
        string jwt = HttpContext.Request.Headers["Authorization"];
        int idCliente = 0;

        if (!string.IsNullOrEmpty(jwt))
        {
            jwt = jwt.Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            if (token.Claims.FirstOrDefault(claim => claim.Type == "userId") is Claim conteudoClaim)
            {
                idCliente = int.Parse(conteudoClaim.Value);
            }
        }

        var agendamentos = _agendamentoRepository.ListarConsultas(idCliente);

        return Ok(agendamentos);
    }

    [Authorize(Policy = "RequireCliente")]
    [HttpPatch]
    public IActionResult AgendarConsulta(int idAgendamento, Especie especie, string nomeAnimal)
    {
        string jwt = HttpContext.Request.Headers["Authorization"];
        int idCliente = 0;

        if (!string.IsNullOrEmpty(jwt))
        {
            jwt = jwt.Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            if (token.Claims.FirstOrDefault(claim => claim.Type == "userId") is Claim conteudoClaim)
            {
                idCliente = int.Parse(conteudoClaim.Value);
            }
        }

        _agendamentoRepository.AgendarConsulta(idAgendamento, especie, nomeAnimal, idCliente);

        return Ok();
    }

    // Endpoints of the Both
    [HttpGet]
    [Route("pesquisarAgendamentos")]
    public IActionResult PesquisarAgendamentos(DateOnly data)
    {
        var agendamentos = _agendamentoRepository.PesquisarAgendamentos(data);

        return Ok(agendamentos);
    }

    [HttpDelete]
    public IActionResult CancelarConsulta(int idAgendamento)
    {
        _agendamentoRepository.CancelarConsulta(idAgendamento);

        return Ok();
    }

    [HttpPatch]
    [Route("finalizarConsulta")]
    public IActionResult FinalizarConsulta(int idAgendamento)
    {
        _agendamentoRepository.FinalizarConsulta(idAgendamento);

        return Ok();
    }
}
