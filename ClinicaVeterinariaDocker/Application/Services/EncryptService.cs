using System.Security.Cryptography;

namespace estudo_final.Application.Services;

public static class EncryptService {
    public static string Encriptar(string senha) {
        // Cria um array para armazenar o salt
        byte[] salt = new byte[32];

        // Cria um número aleatório para o salt
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        // Criptografa a senha utilizando o salt
        using var encriptador = new Rfc2898DeriveBytes(senha, salt, 10000);
        byte[] hash = encriptador.GetBytes(64);
        byte[] hashSenha = new byte[96];

        // Combina o salt com a senha
        Array.Copy(salt, 0, hashSenha, 0, 32);
        Array.Copy(hash, 0, hashSenha, 32, 64);

        // Convert a senha com salt em string
        return Convert.ToBase64String(hashSenha);
    }

    public static bool Validar(string senhaDB, string senhaUsuario) {
        // Transforma a senha em um array de bytes
        byte[] byteSenhaDB = Convert.FromBase64String(senhaDB);
        byte[] salt = new byte[32];
        // Extrai o salt da senha
        Array.Copy(byteSenhaDB, 0, salt, 0, 32);

        // Criptografa a senha que o usuário enviou
        using var encriptador = new Rfc2898DeriveBytes(senhaUsuario, salt, 10000);
        byte[] hash = encriptador.GetBytes(64);
        byte[] hashSenha = new byte[96];

        // Combina o salt com a senha do usuário criptografado
        Array.Copy(salt, 0, hashSenha, 0, 32);
        Array.Copy(hash, 0, hashSenha, 32, 64);

        // Compara a senha do usuário e a senha do DB
        return senhaDB == Convert.ToBase64String(hashSenha);
    }
}
