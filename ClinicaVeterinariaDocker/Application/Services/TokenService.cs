using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace estudo_final.Application.Services;

public class TokenService{
    public static string GerarToken(int idUser, string tipo) {
        var key = Encoding.ASCII.GetBytes(SecretKey.Key);

        var tokenConfig = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new Claim[] {
                new Claim("tipo", tipo),
                new Claim("idUser", idUser.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(3),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenResult = tokenHandler.CreateToken(tokenConfig);
        return tokenHandler.WriteToken(tokenResult);
    }

    public static int RecuperarId(HttpContext httpContext) {
        string jwt = httpContext.Request.Headers["Authorization"];

        int idUser = -1;
        if (!string.IsNullOrEmpty(jwt))
        {
            jwt = jwt.Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            if (token.Claims.FirstOrDefault(claims => claims.Type == "idUser") is Claim objetoClaim)
            {
                idUser = int.Parse(objetoClaim.Value);
            }
        }

        return idUser;
    }
}
