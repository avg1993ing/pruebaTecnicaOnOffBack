using Core.Entities;

namespace Core.Interfaces.Repository
{
    public interface IUtilsFunctionsRepository
    {
        string DecodeMd5(string StringToConvert);
        string GenerateTokenJWT(Users user);
        Task<string> GenerateTokenJWTRecoveryPassword(Users user);
        Task<bool> SMTP(string toEmail, string subject, string token);
        int GetIdUserToken(string Token);
    }
}
