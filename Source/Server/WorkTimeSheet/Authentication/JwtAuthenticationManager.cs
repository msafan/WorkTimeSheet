using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.DTO;
using WorkTimeSheet.Models;

namespace WorkTimeSheet
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly IDbContext _dbContext;

        public JwtAuthenticationManager(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public AuthorizedUser Authenticate(UserDTO user)
        {
            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim> 
            {
                new Claim(ClaimTypes.Name, user.Name.ToString()),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.GroupSid, user.OrganizationId.ToString()) 
            };
            claims.AddRange(user.UserRoles.Select(role => new Claim(ClaimTypes.Role, role.Role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(Constants.AccessTokenTimeOut),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Constants.JwtTokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = Guid.NewGuid().ToString("N");

            _dbContext.RefreshTokens.Add(new RefreshToken
            {
                ExpireDateTime = DateTime.UtcNow.AddDays(14),
                IssueDateTime = DateTime.UtcNow,
                Token = refreshToken,
                UserId = user.Id
            });
            _dbContext.SaveChanges();

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
