using ClinicaVeterinaria.Domain.Models.ClienteAggregate;
using ClinicaVeterinaria.Application.ViewModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicaVeterinaria.Controllers;

[ApiController]
[Route("api/v1/cliente")]
public class ClienteController : Controller
{
    private readonly IClienteRepository _clienteRepository;

    // Setup the Interfaces to be used on this class
    public ClienteController(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
    }

    [HttpPost]
    public IActionResult Cadastro([FromForm][Required] string nome, [Required] string email, 
        [Required] string senha) {
        // Encapsulates the nome, email and senha to send it to the Repository
        var cliente = new Cliente(nome, email, senha);

        // Call the Repository to create the Cliente
        _clienteRepository.Cadastro(cliente);

        return Ok();
    }

    [HttpPatch]
    [Route("pedidoRecuperar")]
    public IActionResult PedidoRecuperarSenha([FromForm][Required] string email)
    {
        string codigo = _clienteRepository.PedidoRecuperarSenha(email);

        if (codigo == "-1") {
            return BadRequest("Email não encontrado!");
        }

        return Ok(codigo);
    }

    [HttpPatch]
    [Route("recuperar")]
    public IActionResult RecuperarSenha([FromForm][Required] string email, [Required] string novaSenha,
        [Required] string codigoValidacao)
    {
        Cliente cliente = new(email, novaSenha);

        bool certo = _clienteRepository.RecuperarSenha(cliente, codigoValidacao);

        if (certo) {
            return Ok();
        }

        return BadRequest("Email não encontrado ou código inválido!");
    }

    [Authorize(Policy = "RequireCliente")]
    [HttpPut]
    [Route("editarPerfil")]
    public IActionResult EditarPerfil([FromForm] ClienteViewModel clienteView) {

        Cliente cliente = new(clienteView.Nome, clienteView.Email, clienteView.Senha);

        string jwt = HttpContext.Request.Headers["Authorization"];
        int idCliente = 0;

        if (!string.IsNullOrEmpty(jwt)) {
            // Remove "Bearer " prefix if it's included in the header
            jwt = jwt.Replace("Bearer ", "");

            // Decode the JWT
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            // Extract the subject claim (for example, the user's ID)
            if (token.Claims.FirstOrDefault(claim => claim.Type == "userId") is Claim subjectClaim) {
                idCliente = int.Parse(subjectClaim.Value);
            }
        }

        _clienteRepository.EditarPerfil(cliente, idCliente);

        return Ok();
    }
}
