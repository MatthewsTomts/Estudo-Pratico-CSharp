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
    public IActionResult CadastrarFuncionario( FuncionarioViewModel funcionarioVM) {
        funcionarioVM.senha = EncryptService.Encriptar(funcionarioVM.senha);

        Funcionario funcionario = new(funcionarioVM.nif, funcionarioVM.nome,
            funcionarioVM.senha, funcionarioVM.cargo, 0);

        try {
            _funcionarioRepository.CadastrarFuncionario(funcionario);
            return Ok(new { message = "Funcionario Cadastrado" });
        } catch (DbUpdateException ex) {
            Console.WriteLine(ex.ToString());
            return BadRequest(new { message = "Nif já cadastrado" });
        } catch (Exception ex) {
            Console.WriteLine(ex.ToString());
            return BadRequest(new { message = "Um erro ocorreu" });
        }
    }

    [HttpDelete]
    [Authorize(Policy = "RequireAdmin")]
    public IActionResult DemitirFuncionario(int nif) {
        _funcionarioRepository.DemitirFuncionario(nif);
        return Ok(new { message = "Funcionario Demitido" });
    }

    [HttpPatch]
    [Authorize(Policy = "RequireAdmin")]
    public IActionResult EditarPerfil( int nif, string? nome, string? senha, CargoFuncionario cargo) {
        if (senha != null) {
            senha = EncryptService.Encriptar(senha);
        }

        Funcionario funcionario = new(nif, nome, senha, cargo);

        try {
            _funcionarioRepository.EditarPerfil(funcionario);
            return Ok(new { message = "Perfil Editado" });
        } catch (Exception ex) {
            Console.WriteLine(ex.ToString());
            return BadRequest(new { message = "Um erro ocorreu" });
        }
    }

    [HttpPost]
    [Route("pesquisarFuncionario")]
    public IActionResult PesquisarFuncionario([Required] int nif) {
        Funcionario funcionario = _funcionarioRepository.PesquisarFuncionario(nif);
        return Ok(funcionario);
    }
}
