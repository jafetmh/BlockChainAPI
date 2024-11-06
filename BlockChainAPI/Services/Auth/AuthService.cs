using BlockChain_DB;
using BlockChain_DB.General;
using BlockChainAPI.Interfaces.IDataService;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlockChainAPI.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            Jwt jwt = _configuration.GetSection("Jwt").Get<Jwt>();
            var byte_key = Encoding.UTF8.GetBytes(jwt.Key);

            var key = new SymmetricSecurityKey(byte_key);
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                }),
                Issuer = jwt.Issuer,
                Audience = jwt.Audience,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = signIn,
            };

            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);

        }
    }
}
