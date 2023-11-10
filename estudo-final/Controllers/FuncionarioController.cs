using estudo_final.Models.FuncionarioAggregate;
using estudo_final.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using estudo_final.Application.Services;
using Microsoft.EntityFrameworkCore;
using static estudo_final.Models.FuncionarioAggregate.Funcionario;
using System.ComponentModel.DataAnnotations;

namespace estudo_final.Controllers;

[ApiController]
[Route("/api/funcionario")]
public class FuncionarioController : Controller {
    private readonly IFuncionarioRepository _funcionarioRepository;

    public FuncionarioController(IFuncionarioRepository funcionarioRepository) {
        _funcionarioRepository = funcionarioRepository;
    }

    [HttpPost]
    [Authorize(Policy = "RequireAdmin")]
    public IActionResult CadastrarFuncionario([FromForm] FuncionarioViewModel funcionarioVM) {
        funcionarioVM.senha = EncryptService.Encriptar(funcionarioVM.senha);

        Funcionario funcionario = new(funcionarioVM.nif, funcionarioVM.nome,
            funcionarioVM.senha, funcionarioVM.cargo, 0);

        try {
            _funcionarioRepository.CadastrarFuncionario(funcionario);
            return Ok();
        } catch (DbUpdateException ex) {
            Console.WriteLine(ex.ToString());
            return BadRequest("Nif já cadastrado");
        } catch (Exception ex) {
            Console.WriteLine(ex.ToString());
            return BadRequest();
        }
    }

    [HttpDelete]
    [Authorize(Policy = "RequireAdmin")]
    public IActionResult DemitirFuncionario(int nif) {
        _funcionarioRepository.DemitirFuncionario(nif);
        return Ok();
    }

    [HttpPatch]
    [Authorize(Policy = "RequireAdmin")]
    public IActionResult EditarPerfil([FromForm] int nif, string? nome, string? senha, CargoFuncionario cargo) {
        if (senha != null) {
            senha = EncryptService.Encriptar(senha);
        }

        Funcionario funcionario = new(nif, nome, senha, cargo);

        try {
            _funcionarioRepository.EditarPerfil(funcionario);
            return Ok();
        } catch (Exception ex) {
            Console.WriteLine(ex.ToString());
            return BadRequest();
        }
    }

    [HttpPost]
    [Route("pesquisarFuncionario")]
    public IActionResult PesquisarFuncionario([FromForm][Required] int nif) {
        Funcionario funcionario = _funcionarioRepository.PesquisarFuncionario(nif);
        return Ok(funcionario);
    }
}
