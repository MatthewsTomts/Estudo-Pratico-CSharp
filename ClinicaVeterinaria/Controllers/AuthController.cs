using ClinicaVeterinaria.Domain.Models.ClienteAggregate;
using ClinicaVeterinaria.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ClinicaVeterinaria.Domain.Models.FuncionarioAggregate;

namespace ClinicaVeterinaria.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : Controller
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IFuncionarioRepository _funcionarioRepository;

    // Setup the Interfaces to be used on this class
    public AuthController(IClienteRepository clienteRepository, IFuncionarioRepository funcionarioRepository) {
        _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
        _funcionarioRepository = funcionarioRepository ?? throw new ArgumentNullException(nameof(funcionarioRepository));
    }

    [HttpPost]
    [Route("login/cliente")]
    public IActionResult LoginCliente(string email, string senha) {
        // Encapsulates the email and senha to send it to the Repository
        Cliente cliente = new(email, senha); 

        // Call the Repository to check the user and return its id
        int? idCliente = _clienteRepository.Logar(cliente); // 'int?' accepts null values

        // Verifies if the Repository returned a valid id
        if (idCliente != 0) {
            // '(int)' changes the 'int?' to 'int'
            var token = TokenService.GenerateToken((int)idCliente, "Cliente"); 
            return Ok(token);
        }

        return BadRequest("Usuário ou senha incorretos!");
    }

    [HttpPost]
    [Route("login/funcionario")]
    public IActionResult LoginFuncionario(int nif, string senha)
    {
        // Encapsulates the email and senha to send it to the Repository
        Funcionario funcionario = new(nif, senha);

        // Call the Repository to check the user and return its id
        Funcionario funcionarioDB = _funcionarioRepository.Logar(funcionario); // 'int?' accepts null values

        // Verifies if the Repository returned a valid id
        if (funcionarioDB != null) {
            // '(int)' changes the 'int?' to 'int'
            var token = TokenService.GenerateToken((int)funcionarioDB.nif, funcionarioDB.cargo.ToString());
            return Ok(token);
        }

        return BadRequest("Usuário ou senha incorretos!");
    }

    [HttpPost]
    [Route("logoff")]
    public IActionResult Logoff()
    {
        // Retrieve the current user's ClaimsPrincipal
        if (HttpContext.User is ClaimsPrincipal user)
        {
            // Find the "userId" claim and extract the token
            var clienteIdClaim = user.FindFirst("userId");
            var tipoClaim = user.FindFirst("tipo");

            if (clienteIdClaim != null)
            {
                // Remove the "userId" claim
                var identity = user.Identity as ClaimsIdentity;
                identity?.RemoveClaim(clienteIdClaim);
            }

            if (tipoClaim != null)
            {
                // Remove the "tipo" claim
                var identity = user.Identity as ClaimsIdentity;
                identity?.RemoveClaim(tipoClaim);
            }
        }

        return Ok();
    }
}
