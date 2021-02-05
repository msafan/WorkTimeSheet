using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using WorkTimeSheet.Authentication;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.DTO;
using WorkTimeSheet.Models;

namespace WorkTimeSheet
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly IDbContext _dbContext;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        private readonly byte[] _key;

        public JwtAuthenticationManager(IDbContext dbContext, IRefreshTokenGenerator refreshTokenGenerator, byte[] key)
        {
            _dbContext = dbContext;
            _refreshTokenGenerator = refreshTokenGenerator;
            _key = key;
        }

        public AuthorizedUser Authenticate(UserDTO user, bool isRefreshTokenRequired = true, DateTime? accessTokenExpiryDate = null)
        {
            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[] { new Claim(ClaimTypes.Name, user.Id.ToString()) }),
                Expires = accessTokenExpiryDate ?? DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(_key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            string refreshToken = string.Empty;
            if (isRefreshTokenRequired)
            {
                refreshToken = _refreshTokenGenerator.GenerateToken();

                _dbContext.RefreshTokens.Add(new RefreshToken
                {
                    ExpireDateTime = DateTime.UtcNow.AddDays(14),
                    IssueDateTime = DateTime.UtcNow,
                    Token = refreshToken,
                    UserId = user.Id
                });
                _dbContext.SaveChanges();
            }

            return new AuthorizedUser
            {
                UserId = user.Id,
                Name = user.Name,
                AccessToken = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken,
                Roles = user.UserRoles.ToList()
            };
        }
    }
}
