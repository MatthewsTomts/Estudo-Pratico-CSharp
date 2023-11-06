using ClinicaVeterinaria.Domain.Models.FuncionarioAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public IActionResult CadastrarFuncionario(int nif)
    {
        _funcionarioRepository.DemitirFuncionario(nif);
        return Ok();
    }
}

