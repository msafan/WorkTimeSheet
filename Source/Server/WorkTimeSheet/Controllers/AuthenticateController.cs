using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WorkTimeSheet.Authentication;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.DTO;
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
            var user = DbContext.Users.FirstOrDefault(x => x.Email == userCredential.Email && x.Password == userCredential.Password);
            if (user == null)
                return Unauthorized();

            var authorizedUser = _authenticationManager.Authenticate(Mapper.Map<UserDTO>(user));
            if (authorizedUser == null)
                return Unauthorized();

            return Ok(authorizedUser);
        }

        [HttpPost("refresh")]
        [ProducesDefaultResponseType(typeof(AuthorizedUser))]
        [AllowAnonymous]
        public IActionResult RefreshToken([FromBody] AuthorizedUser authorizedUser)
        {
            var newAuthorizedUser = _tokenRefresher.Refresh(authorizedUser);
            if (newAuthorizedUser == null)
                return Unauthorized();

            return Ok(newAuthorizedUser);
        }
    }
}
