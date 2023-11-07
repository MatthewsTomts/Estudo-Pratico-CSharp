using ClinicaVeterinaria.Application.ViewModel;
using ClinicaVeterinaria.Domain.Models.ClienteAggregate;
using ClinicaVeterinaria.Domain.Models.FuncionarioAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static ClinicaVeterinaria.Domain.Models.FuncionarioAggregate.Funcionario;

namespace ClinicaVeterinaria.Controllers;

[ApiController]
[Route("/api/adm")]
public class AdministradorController : Controller {
    public IFuncionarioRepository _funcionarioRepository;

    public AdministradorController(IFuncionarioRepository funcionarioRepository) {
        _funcionarioRepository = funcionarioRepository ?? throw new ArgumentNullException(nameof(funcionarioRepository));
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpPost]
    public IActionResult CadastrarFuncionario(int nif, string nome, Cargo Cargo, string senha) {
        Funcionario funcionario = new(nif, nome, senha, Cargo);

        _funcionarioRepository.CadastrarFuncionario(funcionario);

        return Ok();
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpDelete]
    public IActionResult DemitirFuncionario(int nif)
    {
        _funcionarioRepository.DemitirFuncionario(nif);
        return Ok();
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpPut]
    [Route("editarSenha")]
    public IActionResult EditarSenha(int nif, string novaSenha)
    {
        _funcionarioRepository.EditarSenha(nif, novaSenha);

        return Ok();
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpGet]
    [Route("pesquisarFuncionario")]
    public IActionResult PesquisarFuncionario(int nif)
    {
        var funcionario = _funcionarioRepository.PesquisarFuncionario(nif);

        return Ok(funcionario);
    }
}

