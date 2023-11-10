namespace estudo_final.Models.ClienteAggregate;

// Interface que define os metodos relacionados ao cliente
public interface IClienteRepository {
    public int Logar(Cliente cliente);
    public void Cadastro(Cliente cliente);
    public void ApagarPerfil(int idCliente);
    public void EditarPerfil(Cliente cliente);
    public string PedidoRecuperarSenha(string email);
    public void RecuperarSenha(string email, string senha, string codigoValidador);
}
