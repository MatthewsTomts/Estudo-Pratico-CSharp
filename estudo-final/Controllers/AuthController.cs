using estudo_final.Application.Services;
using estudo_final.Models.ClienteAggregate;
using estudo_final.Models.FuncionarioAggregate;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace estudo_final.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController : Controller {
    private readonly IClienteRepository _clienteRepository;
    private readonly IFuncionarioRepository _funcionarioRepository;

    public AuthController(IClienteRepository clienteRepository, IFuncionarioRepository funcionarioRepository) { 
        // Inicia a interface do cliente
        _clienteRepository = clienteRepository;
        _funcionarioRepository = funcionarioRepository;
    }

    [HttpPost]
    [Route("cliente")]
    public IActionResult LoginCliente([FromForm][Required] string email, [Required] string senha) {
        Cliente cliente = new(email, senha);

        try {
            // Verifica se o email e senha estão corretos e retorna o id para criar o JWT
            int id = _clienteRepository.Logar(cliente);

            // Se o id retornado for -1, o email e/ou senha estão incorretos
            if (id != -1) {
                // Gera o token JWT
                var token = TokenService.GerarToken(id, "Cliente");
                // Envia o token junto com uma mensagem
                return Ok(new {
                    message = "Login realizado com sucesso: ",
                    token
                });

            } else {
                return BadRequest("Usuário ou senha incorretos");
            }
        } catch (Exception ex) {
            // Mostra as exceções no console e enviar um BadRequest para o front
            Console.WriteLine(ex.Message);
            return BadRequest();
        }
    }

    [HttpPost]
    [Route("funcionario")]
    public IActionResult LoginFuncioanrio([FromForm][Required] int nif, [Required] string senha) {
        Funcionario funcionario = new(nif, senha);

        try {
            Funcionario funcionarioDB = _funcionarioRepository.Login(funcionario);

            if (funcionarioDB.Nif != -1) {
                var token = TokenService.GerarToken(funcionarioDB.Nif, funcionarioDB.Cargo.ToString());

                return Ok(new
                {
                    message = "Usuário logado com sucesso!",
                    token
                });
            }

            return BadRequest();
        } catch (Exception ex) {
            Console.WriteLine(ex.Message);
            return BadRequest("Usuário ou senha incorretos!");
        }
    }

    [HttpGet]
    [Route("logoff")]
    public IActionResult Logoff() {
        if (HttpContext.User is ClaimsPrincipal User) {
            var userTipoClaim = User.FindFirst("tipo");
            var userIdClaim = User.FindFirst("idUser");

            if (userTipoClaim != null) {
                var identity = User.Identity as ClaimsIdentity;
                identity?.RemoveClaim(userTipoClaim);
            }

            if(userIdClaim != null) {
                var identity = User.Identity as ClaimsIdentity;
                identity?.RemoveClaim(userIdClaim);
            }
        }
        return Ok();
    }
}
