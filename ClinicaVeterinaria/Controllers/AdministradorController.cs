using static ClinicaVeterinaria.Domain.Models.FuncionarioAggregate.Funcionario;
using ClinicaVeterinaria.Domain.Models.FuncionarioAggregate;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult CadastrarFuncionario([FromForm][Required] int nif, [Required] string nome,
        [Required] Cargo Cargo, [Required] string senha) {
        Funcionario funcionario = new(nif, nome, senha, Cargo);

        _funcionarioRepository.CadastrarFuncionario(funcionario);

        return Ok();
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpDelete]
    public IActionResult DemitirFuncionario([Required] int nif)
    {
        _funcionarioRepository.DemitirFuncionario(nif);
        return Ok();
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpPut]
    [Route("editarSenha")]
    public IActionResult EditarSenha([Required] int nif, [Required] string novaSenha)
    {
        _funcionarioRepository.EditarSenha(nif, novaSenha);

        return Ok();
    }

    [Authorize(Policy = "RequireAdmin")]
    [HttpGet]
    [Route("pesquisarFuncionario")]
    public IActionResult PesquisarFuncionario([Required] int nif)
    {
        var funcionario = _funcionarioRepository.PesquisarFuncionario(nif);

        return Ok(funcionario);
    }
}

