using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WorkTimeSheet.Authentication;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.DTO;
using WorkTimeSheet.Excepions;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Controllers
{
    [Route("api/authenticate")]
    public class AuthenticateController : ApiControllerBase
    {
        private readonly IJwtAuthenticationManager _authenticationManager;
        private readonly ITokenRefresher _tokenRefresher;

        public AuthenticateController(IDbContext dbContext, IMapper mapper, IJwtAuthenticationManager authenticationManager, ITokenRefresher tokenRefresher) : base(dbContext, mapper)
        {
            _authenticationManager = authenticationManager;
            _tokenRefresher = tokenRefresher;
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(AuthorizedUser))]
        [AllowAnonymous]
        public IActionResult Post([FromBody] UserCredential userCredential)
        {
            var user = DbContext.Users
                .Include(x => x.UserRoleMappings)
                .ThenInclude(x => x.UserRole)
                .FirstOrDefault(x => x.Email == userCredential.Email);
            if (user == null)
                throw new InvalidUserException("Invalid Email Id");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userCredential.Password, user.Salt);
            if (user.Password != hashedPassword)
                throw new InvalidUserException("Invalid Password");

            var authorizedUser = _authenticationManager.Authenticate(Mapper.Map<UserDTO>(user));
            if (authorizedUser == null)
                throw new InvalidUserException();

            return Ok(authorizedUser);
        }

        [HttpPost("refresh")]
        [ProducesDefaultResponseType(typeof(AuthorizedUser))]
        [AllowAnonymous]
        public IActionResult RefreshToken([FromBody] AuthorizedUser authorizedUser)
        {
            var newAuthorizedUser = _tokenRefresher.Refresh(authorizedUser);
            if (newAuthorizedUser == null)
                throw new InvalidUserException();

            return Ok(newAuthorizedUser);
        }
    }
}
