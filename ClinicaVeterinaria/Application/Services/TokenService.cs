using ClinicaVeterinaria.Domain.Models.ClienteAggregate;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace ClinicaVeterinaria.Application.Services;

public class TokenService
{
    public static object GenerateToken(int idUser, string tipo)
    {
        var key = Encoding.ASCII.GetBytes(ApiKey.Secret);

        var tokenConfig = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("userId", idUser.ToString()),
                    new Claim("tipo", tipo),
                }
            ),
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenConfig);
        var tokenString = tokenHandler.WriteToken(token);

        return new { token = tokenString };
    }
}
