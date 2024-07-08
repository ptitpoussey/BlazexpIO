using BaseLib.DTOs;
using BaseLib.Responses;

namespace ClientLib.Services.Interfaces
{
    public interface IUserAccountService
    {
        Task<HandleGeneralResponse> CreateAsync(RegisterModel user);
        Task<HandleLoginResponse> SignInAsync(LoginModel user);
        Task<HandleLoginResponse> RefreshTokenAsync(RefreshToken refreshToken);
    }
}
