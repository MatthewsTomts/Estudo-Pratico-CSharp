using System.ComponentModel.DataAnnotations;
using estudo_final.Models.ClienteAggregate;
using estudo_final.Application.ViewModel;
using Microsoft.AspNetCore.Authorization;
using estudo_final.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace estudo_final.Controllers;

[ApiController]
[Route("/api/cliente")]
public class ClienteController : Controller {
    private readonly IClienteRepository _clienteRepository;

    public ClienteController(IClienteRepository clienteRepository) {
        _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
    }

    [HttpPost]
    public IActionResult Cadastro([FromForm][Required] ClienteViewModel clienteView) {
        // Criptografa a senha
        clienteView.senha = EncryptService.Encriptar(clienteView.senha);

        // Gera um objeto Cliente para fazer o transporte dos dados
        Cliente cliente = new(clienteView.nome, clienteView.email, clienteView.senha);

        try {
            // Cadastra o Cliente no banco
            _clienteRepository.Cadastro(cliente);
            return Ok();
        } catch (DbUpdateException e) {
            return BadRequest("Email já cadastrado");
        } catch (Exception e) { 
            // No caso de algum erro durante o cadastro, mostra no console e manda um BadRequest pro front
            Console.WriteLine(e.Message);
            return BadRequest();
        }
    }

    [HttpDelete]
    [Authorize(Policy = "RequireCliente")]
    public IActionResult ApagarPerfil() {
        int idCliente = TokenService.RecuperarId(HttpContext);

        if (idCliente != -1) {
            try {
                _clienteRepository.ApagarPerfil(idCliente);
                return Ok();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return BadRequest("Ocorreu um erro ao apagar o perfil");
            }
        }
        return BadRequest("Erro ao verificar JWT");
    }

    [HttpPatch]
    [Authorize(Policy="RequireCliente")]
    public IActionResult EditarPerfil([FromForm] string? email, string? nome, string? senha) {
        int idCliente = TokenService.RecuperarId(HttpContext);
        Cliente cliente = new(idCliente, email, senha, nome);

        if (idCliente != -1) {
            try {
                _clienteRepository.EditarPerfil(cliente);
                return Ok();
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return BadRequest("Erro ao editar perfil");
            }
        }
        return BadRequest("Erro ao verificar JWT");

    }
}
