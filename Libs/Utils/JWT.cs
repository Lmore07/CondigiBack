using CondigiBack.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CondigiBack.Libs.Utils
{

    public class JWT
    {

        public string generateToken(User user)
        {
            string keyJWT = Environment.GetEnvironmentVariable("KEY_JWT")??"secret";

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.UserType.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyJWT));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtConfig = new JwtSecurityToken(
                issuer: "CondigiBack",
                audience: "CondigiBack",
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
