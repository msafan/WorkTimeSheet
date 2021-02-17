using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.DTO;
using WorkTimeSheet.Excepions;

namespace WorkTimeSheet.Controllers
{
    [Route("api/accesstoken")]
    public class AccessTokenController : ApiControllerBase
    {
        public AccessTokenController(IDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        [HttpGet]
        [Authorize(Roles = Constants.UserRoleOwner)]
        public IActionResult GetAll()
        {
            var query = DbContext.AccessTokens
                .Where(x => x.UserId == CurrentUserId).ToList();

            return Ok(Mapper.Map<List<AccessTokenDTO>>(query));
        }

        [HttpPost]
        [Authorize(Roles = Constants.UserRoleOwner)]
        public IActionResult Post(AccessTokenDTO accessTokenDTO)
        {
            var accessToken = new AccessToken
            {
                ApiKey = Guid.NewGuid().ToString("N"),
                AppName = accessTokenDTO.AppName,
                UserId = CurrentUserId,
                OrganizationId = CurrentUserOrganizationId
            };

            DbContext.AccessTokens.Add(accessToken);
            DbContext.SaveChanges();

            accessToken = DbContext.AccessTokens.FirstOrDefault(x => x.Id == accessToken.Id);

            return Ok(Mapper.Map<AccessTokenDTO>(accessToken));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Constants.UserRoleOwner)]
        public IActionResult Delete(int id)
        {
            var accessToken = DbContext.AccessTokens.FirstOrDefault(x => x.Id == id);
            if (accessToken == null)
                throw new DataNotFoundException($"Access token / Api key not found with Id: {id}");

            DbContext.AccessTokens.Remove(accessToken);
            DbContext.SaveChanges();

            return NoContent();
        }
    }
}
