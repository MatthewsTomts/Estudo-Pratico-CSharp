using ClinicaVeterinaria.Domain.Models.ClienteAggregate;
using ClinicaVeterinaria.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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
    public IActionResult Cadastro([FromForm] ClienteViewModel clienteView)
    {
        // Encapsulates the nome, email and senha to send it to the Repository
        var cliente = new Cliente(clienteView.Nome, clienteView.Email, clienteView.Senha);

        // Call the Repository to create the Cliente
        _clienteRepository.Cadastro(cliente);

        return Ok();
    }

    [HttpPatch]
    [Route("pedidoRecuperar")]
    public IActionResult PedidoRecuperarSenha([FromForm] string email)
    {
        string codigo = _clienteRepository.PedidoRecuperarSenha(email);

        if (codigo == "-1") {
            return BadRequest("Email não encontrado!");
        }

        return Ok(codigo);
    }

    [HttpPatch]
    [Route("recuperar")]
    public IActionResult RecuperarSenha([FromForm] string email, string novaSenha, string codigoValidacao)
    {
        Cliente cliente = new(email, novaSenha);

        bool certo = _clienteRepository.RecuperarSenha(cliente, codigoValidacao);

        if (certo) {
            return Ok();
        }

        return BadRequest("Email não encontrado ou código inválido!");
    }
}
