namespace ClinicaVeterinaria.Domain.Models.ClienteAggregate;

public interface IClienteRepository
{
    public void Cadastro(Cliente cliente);
    public string PedidoRecuperarSenha(string email);
    public bool RecuperarSenha(Cliente cliente, string codigoValidacao);
    public int Logar(Cliente cliente);
    public void EditarPerfil(Cliente cliente, int idCliente);
    public void ApagarPerfil(int idCliente);
}
