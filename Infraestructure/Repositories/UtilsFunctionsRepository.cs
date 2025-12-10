
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MimeKit;

namespace Infraestructure.Repositories
{
    public class UtilsFunctionsRepository : IUtilsFunctionsRepository
    {
        public SmtpClient cliente;
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

        public async Task<string> GenerateTokenJWTRecoveryPassword(Users user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKeyRecoveryPassword"]));

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.id.ToString()));
            claims.AddClaim(new Claim("idUser", user.id.ToString()));
            claims.AddClaim(new Claim("nameUser", user.NameUser));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]));
            claims.AddClaim(new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(createdToken);
        }

        public int GetIdUserToken(string Token)
        {
            if (Token != null)
            {
                try
                {
                    string formatToken = Token.Remove(0, 7);
                    SecurityToken securityToken = null;
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKeyRecoveryPassword"]));

                    TokenValidationParameters validationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = key,
                        ValidAudience = _configuration["Jwt:Audience"],
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        LifetimeValidator = this.Lifetimevalidator

                    };
                    var tokendata = tokenHandler.ValidateToken(formatToken, validationParameters, out securityToken);
                    var arrayDataToken = tokendata.Identities.Select(x => x.Claims).ToArray();
                    foreach (var item in arrayDataToken)
                    {
                        foreach (var item1 in item)
                        {
                            if (item1.Type == "idUser")
                            {
                                return Int32.Parse(item1.Value);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    EntityBaseException entityBaseException = new EntityBaseException();
                    entityBaseException.Message = $"Error valid token : {e.Message}";
                    entityBaseException.Name = "Error valid token";

                    UnauthorizedBusinessException ex = new UnauthorizedBusinessException(entityBaseException);
                    throw ex;
                }

            }
            return 0;
        }

        public bool Lifetimevalidator(DateTime? notBefore,
                              DateTime? expires,
                              SecurityToken securityToken,
                              TokenValidationParameters tokenValidationParameters)
        {
            var valid = false;
            if (expires.HasValue && DateTime.UtcNow < expires)
            {
                valid = true;
            }
            return valid;
        }
        public async Task<bool> SMTP(string toEmail, string subject, string token)
        {
            try
            {
                // 1. Crear el mensaje
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Tareas", "no-reply@tareas.com"));
                message.To.Add(new MailboxAddress(toEmail, toEmail));
                message.Subject = subject;

                // 2. Crear la plantilla HTML usando los estilos de tu app
                string htmlBody = $@"
            <html>
            <head>
                <style>
                    html, body {{
                        height: 100%;
                        margin: 0;
                        padding: 0;
                        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                        background-color: #f8f9fa;
                    }}
                    .container {{
                        display: flex;
                        justify-content: center;
                        align-items: center;
                        height: 100vh;
                    }}
                    .card {{
                        background-color: white;
                        padding: 2rem;
                        border-radius: 12px;
                        box-shadow: 0 10px 30px rgba(0,0,0,0.1);
                        max-width: 500px;
                        text-align: center;
                    }}
                    .btn {{
                        display: inline-block;
                        padding: 12px 25px;
                        margin-top: 20px;
                        font-size: 1rem;
                        font-weight: 600;
                        color: white;
                        background: linear-gradient(90deg, #34d058, #28a745);
                        border-radius: 8px;
                        text-decoration: none;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='card'>
                        <h2>¡Bienvenido a Tareas!</h2>
                        <p>Para activar tu cuenta, haz clic en el botón a continuación:</p>
                        <a class='btn' href='{_configuration["SMTP:domain"]}?token={token}'>Confirmar Correo</a>
                        <p style='margin-top:15px; font-size:0.9rem; color:#6c757d'>
                            Este enlace expirará en 30 minutos.
                        </p>
                    </div>
                </div>
            </body>
            </html>";

                message.Body = new TextPart("html") { Text = htmlBody };

                // 3. Configurar conexión SMTP
                using var client = new MailKit.Net.Smtp.SmtpClient();
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync(_configuration["SMTP:Server"], int.Parse(_configuration["SMTP:Port"]), MailKit.Security.SecureSocketOptions.StartTls);

                // Credenciales
                await client.AuthenticateAsync(_configuration["SMTP:User"], _configuration["SMTP:Password"]);

                // 4. Enviar correo
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                // Aquí puedes loguear ex.Message
                return false;
            }
        }
    }
}
