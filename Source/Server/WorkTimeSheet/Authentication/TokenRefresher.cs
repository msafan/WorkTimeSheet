﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.DTO;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Authentication
{
    public class TokenRefresher : ITokenRefresher
    {
        private readonly IDbContext _dbContext;
        private readonly IJwtAuthenticationManager _authenticationManager;
        private readonly IMapper _mapper;

        public TokenRefresher(IDbContext dbContext, IJwtAuthenticationManager authenticationManager, IMapper mapper)
        {
            _dbContext = dbContext;
            _authenticationManager = authenticationManager;
            _mapper = mapper;
        }

        public AuthorizedUser Refresh(AuthorizedUser authorizedUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var pricipal = tokenHandler.ValidateToken(authorizedUser.AccessToken, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Constants.JwtTokenKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
            }, out var validatedToken) ;

            var jwtToken = validatedToken as JwtSecurityToken;
            if (jwtToken == null)
                throw new SecurityTokenException("Invalid access token");

            var userId = string.IsNullOrEmpty(pricipal.Identity.Name) ? -1 : int.Parse(pricipal.Identity.Name);

            var refreshToken = _dbContext.RefreshTokens.FirstOrDefault(x => x.UserId == userId && x.Token == authorizedUser.RefreshToken);
            if (refreshToken == null)
                throw new SecurityTokenException("Invalid refresh token");

            var user = _dbContext.Users.Include(x => x.UserRoleMappings).ThenInclude(x => x.UserRole).FirstOrDefault(x => x.Id == refreshToken.UserId);
            if (user == null)
                throw new SecurityTokenException("Invalid user");

            _dbContext.RefreshTokens.Remove(refreshToken);
            _dbContext.SaveChanges();

            return _authenticationManager.Authenticate(_mapper.Map<UserDTO>(user));
        }
    }
}
