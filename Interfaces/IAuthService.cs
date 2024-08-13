using AWING.TreasureHuntAPI.Models.DTOs;

namespace AWING.TreasureHuntAPI.Interfaces
{
    public interface IAuthService
    {
        Task<bool> Register(UserRegisterDto userRegisterDto);
        Task<string> Login(UserLoginDto userLoginDto);
        Task<bool> UserExists(string username);
    }
}
