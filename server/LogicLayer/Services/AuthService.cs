using LogicLayer.Helpers;
using LogicLayer.Interfaces;
using LogicLayer.InterfacesOut.Account;
using LogicLayer.Models;
using LogicLayer.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Services
{
    /// <summary>
    /// Authentication service
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly AppSettings _appSettings;
        private readonly ICacheService _cache;
        private readonly IAccountUnitOfWork _userUnitOrWork;
        private readonly IJwtService _jwtService;

        public AuthService(IOptions<AppSettings> appSettings, ICacheService cache, IAccountUnitOfWork userUnitOrWork, IJwtService jwtService)
        {
            _appSettings = appSettings.Value;
            _cache = cache;
            _userUnitOrWork = userUnitOrWork;
            _jwtService = jwtService;
        }

        public async Task<bool> RegisterAsync(UserModel userModel)
        {
            // Check if the user exists with this email

            UserModel candidate = await _userUnitOrWork.Users.FindAsync(u => u.Email == userModel.Email);

            if (candidate != null)
                return false;

            userModel.Id = Guid.NewGuid().ToString();
            userModel.PasswordHash = GetPasswordHash(userModel.Password, _appSettings.Secret);

            // Adding user with "User" role db transaction

            _userUnitOrWork.BeginTransaction();

            try
            {
                await _userUnitOrWork.Users.AddAsync(userModel);
                var roleModel = await _userUnitOrWork.Roles.FindAsync(r => r.Name == StringHelper.UserRoleName);
                var userRoleModel = new UserRoleModel { UserId = userModel.Id, RoleId = roleModel.Id };
                await _userUnitOrWork.UserRoles.AddAsync(userRoleModel);
                await _userUnitOrWork.SaveAsync();
                _userUnitOrWork.Commit();
            }
            catch (Exception ex)
            {
                _userUnitOrWork.Rollback();                     
                ExceptionDispatchInfo.Capture(ex).Throw();
            }

            return true;
        }

        public async Task<TokenModel> AuthenticateAsync(UserModel userModel)
        {
            UserModel currUserModel = await _userUnitOrWork.Users.GetUserWithRolesAsync(u => u.Email == userModel.Email && u.PasswordHash == GetPasswordHash(userModel.Password, _appSettings.Secret));

            if (currUserModel == null)
            {
                return null;
            }

            var jwtToken = _jwtService.GenerateToken(currUserModel);
            var refreshToken = _jwtService.GenerateRefreshToken(currUserModel.Id);
            await _cache.SetAsync<RefreshTokenModel>(refreshToken.Token, refreshToken);
            return new TokenModel { UserId = currUserModel.Id, Token = jwtToken, RefreshToken = refreshToken.Token, Roles = currUserModel.Roles };
        }

        public async Task<TokenModel> RefreshTokenAsync(TokenModel tokenModel)
        {
            var refreshToken = await _cache.GetAsync<RefreshTokenModel>(tokenModel.RefreshToken);

            if (refreshToken == null || refreshToken.Expires <= DateTime.UtcNow)
            {
                return null;
            }

            var user = await _userUnitOrWork.Users.GetUserWithRolesAsync(u => u.Id == refreshToken.UserId);

            if (user == null)
            {
                return null;
            }

            var jwtToken = _jwtService.GenerateToken(user);
            return new TokenModel { Token = jwtToken };
        }

        public async Task RevokeToken(TokenModel tokenModel)
        {
            await _cache.ClearCacheAsync(tokenModel.RefreshToken);
        }

        private static string GetPasswordHash(string password, string secret)
        {
            string computedHash;

            using (var hmac = new System.Security.Cryptography.HMACSHA512(Encoding.UTF8.GetBytes(secret)))
            {
                computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }

            return computedHash;
        }
    }
}
