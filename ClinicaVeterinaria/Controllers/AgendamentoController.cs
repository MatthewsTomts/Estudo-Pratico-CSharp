using ClinicaVeterinaria.Domain.Models.AgendamentoAggreagate;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicaVeterinaria.Controllers;

[ApiController]
[Route("/api/agendamento")]
public class AgendamentoController : Controller
{
    public IAgendamentoRepository _agendamentoRepository;

    public AgendamentoController(IAgendamentoRepository agendamentoRepository) {
        _agendamentoRepository = agendamentoRepository;
    }

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
}
