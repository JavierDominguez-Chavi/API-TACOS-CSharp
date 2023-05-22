using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JWTTokens;

public class JwtTokenHandler
{
    public const String JWT_SECURITY_Key = 
        "i-like-2_eat??tacos1!!!!!fdsaf+ewr5f+645";
    private const int JWT_TOKEN_MINS = 30;

    public JwtTokenHandler()
    {

    }

    public Tuple<String, Int32> GenerarToken(String login, String nombreCompleto, String _Id)
    {
        var expiraToken = DateTime.Now.AddHours(6).AddMinutes(JWT_TOKEN_MINS) ;
        var tokeney = Encoding.ASCII.GetBytes(JWT_SECURITY_Key);
        var identidad = new ClaimsIdentity(new List<Claim>{
            new Claim(JwtRegisteredClaimNames.NameId, login),
            new Claim(ClaimTypes.DateOfBirth, DateTime.Now.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, nombreCompleto),
            });
        var credencialesFirma = new SigningCredentials(new SymmetricSecurityKey(tokeney),
                                    SecurityAlgorithms.HmacSha256Signature);
        var descriptorTokenSeguridad = new SecurityTokenDescriptor
        {
            Subject = identidad,
            Expires = expiraToken,
            SigningCredentials = credencialesFirma
        };

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var securityToken = jwtSecurityTokenHandler.CreateToken(descriptorTokenSeguridad);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);
        return new Tuple<String, Int32>(token, JWT_TOKEN_MINS*60);
    }
}