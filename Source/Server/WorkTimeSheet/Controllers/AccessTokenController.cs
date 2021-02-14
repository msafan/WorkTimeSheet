using AutoMapper;
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
        public IActionResult GetAll()
        {
            var query = DbContext.AccessTokens
                .Where(x => x.UserId == CurrentUser.Id).ToList();

            return Ok(Mapper.Map<List<AccessTokenDTO>>(query));
        }

        [HttpPost]
        public IActionResult Post(AccessTokenDTO accessTokenDTO)
        {
            var accessToken = new AccessToken
            {
                ApiKey = Guid.NewGuid().ToString("N"),
                AppName = accessTokenDTO.AppName,
                UserId = CurrentUser.Id
            };

            DbContext.AccessTokens.Add(accessToken);
            DbContext.SaveChanges();

            accessToken = DbContext.AccessTokens.FirstOrDefault(x => x.Id == accessToken.Id);

            return Ok(Mapper.Map<AccessTokenDTO>(accessToken));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var accessToken = DbContext.AccessTokens.FirstOrDefault(x => x.Id == id && x.UserId == CurrentUser.Id);
            if (accessToken == null)
                throw new DataNotFoundException($"Access token / Api key not found with Id: {id}");

            DbContext.AccessTokens.Remove(accessToken);
            DbContext.SaveChanges();

            return NoContent();
        }
    }
}
