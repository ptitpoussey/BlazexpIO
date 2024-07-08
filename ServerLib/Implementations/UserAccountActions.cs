using BaseLib.DTOs;
using BaseLib.Entities;
using BaseLib.Responses;
using BlazexpIO.Utils.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ServerLib.Data;
using ServerLib.Helpers;
using ServerLib.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ServerLib.Implementations
{
    public class UserAccountActions(IOptions<JwtHelper> config, BlazexDbContext context) : IUserAccountActions
    {
        public async Task<HandleGeneralResponse> CreateAsync(RegisterModel user)
        {
            if (user == null)
            {
                return new HandleGeneralResponse(false, "User cannot be null");
            }
            var checkUser = await FindUserByUsername(user.UserName);
            if (checkUser != null) return new HandleGeneralResponse(false, "User already exist.");
            user!.Authority = AuthorityType.Admin;
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password!);
            var result = new ApplicationUser
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                Authority = user.Authority
            };
            await InsertAccount(result);
            return new HandleGeneralResponse(true, "User created successfully");
        }

        internal object GenerateToken(ApplicationUser checkUser, string? roleName)
        {
            if (checkUser == null)
                throw new ArgumentNullException(nameof(checkUser));
            if (string.IsNullOrEmpty(config.Value.Issuer))
                throw new ArgumentNullException(nameof(config.Value.Key));
            if (string.IsNullOrEmpty(config.Value.Issuer))
                throw new ArgumentNullException(nameof(config.Value.Issuer));
            if (string.IsNullOrEmpty(config.Value.Audience))
                throw new ArgumentNullException(nameof(config.Value.Audience));
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key));
            var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, checkUser.Id.ToString()),
                new Claim(ClaimTypes.Name, checkUser.UserName!),
                new Claim(ClaimTypes.Email, checkUser.Email!),
                new Claim(ClaimTypes.Role, checkUser.Authority.ToString())
            };
            var token = new JwtSecurityToken(issuer: config.Value.Issuer, audience: config.Value.Audience, claims: userClaims, expires: DateTime.Now.AddHours(1), signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public async Task<HandleLoginResponse> SignInAsync(LoginModel user)
        {
            if (user == null) return new HandleLoginResponse(false, "User cannot be null");
            var checkUser = await FindUserByUsername(user.UserName!);
            if (checkUser == null) return new HandleLoginResponse(false, "User not found");
            if (!BCrypt.Net.BCrypt.Verify(user.Password!, checkUser.Password!)) return new HandleLoginResponse(false, "Invalid password");

            var userAuthority = await FindUserAuthority(checkUser.Id);
            if (userAuthority == null) return new HandleLoginResponse(false, "User authority not found");
            var authorityName = await FindUserAuthorityName(checkUser.Id);
            if (authorityName == null) return new HandleLoginResponse(false, "User authority not found");

            var token = GenerateToken(checkUser, authorityName);
            string refreshToken = GenerateRefreshToken();
            var userRefresh = context.RefreshTokenInfos.FirstOrDefault(x => x.UserId == checkUser.Id);
            if (userRefresh is not null) 
            {
                userRefresh!.Token = refreshToken;
                await context.SaveChangesAsync();
            }
            await AddToDatabase(new RefreshTokenInfos { Token = refreshToken, UserId = checkUser.Id } );
            await context.SaveChangesAsync();
            return new HandleLoginResponse(true, "Login successful", token.ToString(), refreshToken);
        }
        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        private async Task<ApplicationUser> FindUserByEmail(string email)
        {
            try
            {
                var user = await context.ApplicationUsers.FirstOrDefaultAsync(x => x.Email!.ToLower().Equals(email!.ToLower()));
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task AddToDatabase<T>(T model) => await context.AddAsync(model!);
        private async Task<ApplicationUser> FindUserByUsername(string username) => await context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName!.Equals(username!));
        private async Task<ApplicationUser> FindUserAuthority(int userId) => await context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == userId);
        private async Task<string> FindUserAuthorityName(int userId)
        {
            var user = await context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                return user.Authority.ToString();
            }
            return AuthorityType.None.ToString();
        }
        public async Task<HandleLoginResponse> RefreshTokenAsync(RefreshToken refreshToken)
        {
            if (refreshToken == null) return new HandleLoginResponse(false, "Invalid token");
            var findToken = await context.RefreshTokenInfos.FirstOrDefaultAsync(x => x.Token == refreshToken.Token);
            if (findToken == null) return new HandleLoginResponse(false, "Refresh token required.");
            
            var user = await context.ApplicationUsers.FirstOrDefaultAsync(x => x.Id == findToken.UserId);
            if (user == null) return new HandleLoginResponse(false, "Refresh token could not be found for this user.");
            

            var userAuthority = await FindUserAuthority(user.Id);
            var authorityName = await FindUserAuthorityName(user.Id);
            var jwtToken = GenerateToken(user, authorityName);
            var newRefreshToken = GenerateRefreshToken();


            var updateRefreshToken = await context.RefreshTokenInfos.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (updateRefreshToken == null) return new HandleLoginResponse(false, "Refresh token could not be found for this user.");
            updateRefreshToken.Token = newRefreshToken;

            await context.SaveChangesAsync();
            return new HandleLoginResponse(true, "Token Refreshed successfully", jwtToken.ToString(), newRefreshToken);
        }
        private async Task<T> InsertAccount<T>(T model)
        {
            var result = await context.AddAsync(model!);
            await context.SaveChangesAsync();
            return (T)result.Entity;
        }
    }
}