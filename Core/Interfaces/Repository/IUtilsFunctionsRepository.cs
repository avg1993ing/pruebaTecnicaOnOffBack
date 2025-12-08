using Core.Entities;

namespace Core.Interfaces.Repository
{
    public interface IUtilsFunctionsRepository
    {
        string DecodeMd5(string StringToConvert);
        string GenerateTokenJWT(Users user);
    }
}
