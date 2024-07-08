using BaseLib.DTOs;
using BaseLib.Entities;
using BaseLib.Responses;

namespace ServerLib.Repositories
{
    public interface IUserAccountActions
    {
        Task<HandleGeneralResponse> CreateAsync(RegisterModel user);
        Task<HandleLoginResponse> SignInAsync(LoginModel user);
        Task<HandleLoginResponse> RefreshTokenAsync(RefreshToken refreshToken);
    }
}
