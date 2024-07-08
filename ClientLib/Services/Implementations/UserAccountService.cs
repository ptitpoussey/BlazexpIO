using BaseLib.DTOs;
using BaseLib.Responses;
using ClientLib.Services.Interfaces;

namespace ClientLib.Services.Implementations
{
    public class UserAccountService : IUserAccountService
    {
        public Task<HandleGeneralResponse> CreateAsync(RegisterModel user)
        {
            throw new NotImplementedException();
        }   

        public Task<HandleLoginResponse> RefreshTokenAsync(RefreshToken refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<HandleLoginResponse> SignInAsync(LoginModel user)
        {
            throw new NotImplementedException();
        }
    }
}
