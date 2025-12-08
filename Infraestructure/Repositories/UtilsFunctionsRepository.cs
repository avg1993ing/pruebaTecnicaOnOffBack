
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infraestructure.Repositories
{
    public class UtilsFunctionsRepository : IUtilsFunctionsRepository
    {
        private IConfiguration? _configuration;
        public UtilsFunctionsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string DecodeMd5(string StringToConvert)
        {
            try
            {
                byte[] BtClearBytes;
                BtClearBytes = new UnicodeEncoding().GetBytes(StringToConvert);
                byte[] BtHashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(BtClearBytes);
                return BitConverter.ToString(BtHashedBytes);
            }
            catch (Exception)
            {
                UnauthorizedBusinessException ex = new UnauthorizedBusinessException("Error encript MD5 in UtilsFunctionsRepository");
                throw ex;
            }
        }

        public string GenerateTokenJWT(Users user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.id.ToString()));
            claims.AddClaim(new Claim("idUser", user.id.ToString()));
            claims.AddClaim(new Claim("nameUser", user.NameUser));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.Now.AddDays(30),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(createdToken);
        }
    }
}
