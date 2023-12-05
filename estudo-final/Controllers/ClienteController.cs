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
    public IActionResult Cadastro([Required] ClienteViewModel clienteView) {
        // Criptografa a senha
        clienteView.senha = EncryptService.Encriptar(clienteView.senha);

        // Gera um objeto Cliente para fazer o transporte dos dados
        Cliente cliente = new(clienteView.nome, clienteView.email, clienteView.senha);

        try {
            // Cadastra o Cliente no banco
            _clienteRepository.Cadastro(cliente);
            return Ok();
        } catch (DbUpdateException e) {
            return BadRequest(new { message = "Email já cadastrado" });
        } catch (Exception e) { 
            // No caso de algum erro durante o cadastro, mostra no console e manda um BadRequest pro front
            Console.WriteLine(e.Message);
            return BadRequest(new { message = "Um erro ocorreu" });
        }
    }

    [HttpGet]
    [Authorize(Policy = "RequireCliente")]
    public IActionResult Dados()
    {
        int idCliente = TokenService.RecuperarId(HttpContext);

        if (idCliente != -1)
        {
            try
            {
                return Ok(_clienteRepository.Dados(idCliente));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(new { message = "Ocorreu um erro ao apagar o perfil" });
            }
        }
        return BadRequest(new { message = "Erro ao verificar JWT" });
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
                return BadRequest(new { message = "Ocorreu um erro ao apagar o perfil" });
            }
        }
        return BadRequest(new { message = "Erro ao verificar JWT" });
    }

    [HttpPatch]
    [Authorize(Policy = "RequireCliente")]
    [Route("Perfil")]
    public IActionResult EditarPerfil(string? email, string? nome) {
        int idCliente = TokenService.RecuperarId(HttpContext);
        Cliente cliente = new(idCliente, email, nome);
            
        if (idCliente != -1) {
            try {
                _clienteRepository.EditarPerfil(cliente);
                return Ok(new { message = "Perfil alterado com sucesso" });
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return BadRequest(new { message = "Erro ao editar perfil" });
            }
        }
        return BadRequest(new { message = "Erro ao verificar JWT" });

    }

    [HttpPatch]
    [Route("Senha")]
    public IActionResult EditarSenha(string senha, int idCliente)
    {
        senha = EncryptService.Encriptar(senha);
        Cliente cliente = new(idCliente, senha);

        if (idCliente != -1)
        {
            try
            {
                _clienteRepository.EditarSenha(cliente);
                return Ok(new { message = "Senha editada com sucesso" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(new { message = "Erro ao editar senha" });
            }
        }
        return BadRequest(new { message = "Erro ao verificar JWT" });

    }
}
