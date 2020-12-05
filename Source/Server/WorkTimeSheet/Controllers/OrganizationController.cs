using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.DTO;

namespace WorkTimeSheet.Controllers
{
    [Route("api/organization")]
    public class OrganizationController : ApiControllerBase
    {
        public OrganizationController(IDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        [HttpGet]
        [ProducesDefaultResponseType(typeof(IEnumerable<OrganizationDTO>))]
        public IActionResult GetAll()
        {
            var organizations = DbContext.Organizations.Include(x => x.Users).Include(x => x.Projects).ToList();
            return Ok(Mapper.Map<IEnumerable<OrganizationDTO>>(organizations));
        }

        [HttpGet("{id}")]
        [ProducesDefaultResponseType(typeof(OrganizationDTO))]
        public IActionResult GetById(int id)
        {
            var organization = DbContext.Organizations.Include(x => x.Users).Include(x => x.Projects).FirstOrDefault(x => x.Id == id);
            if (organization == null)
                return NotFound();
            return Ok(Mapper.Map<OrganizationDTO>(organization));
        }

        [HttpPost]
        [ProducesDefaultResponseType(typeof(OrganizationDTO))]
        public IActionResult Post([FromBody] Organization organization)
        {
            DbContext.Organizations.Add(organization);
            DbContext.SaveChanges();
            return Ok(Mapper.Map<OrganizationDTO>(organization));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesDefaultResponseType(typeof(OrganizationDTO))]
        public IActionResult Register([FromBody] Organization organization)
        {
            DbContext.Organizations.Add(organization);
            DbContext.SaveChanges();

            DbContext.UserRoleMappings.Add(new UserRoleMapping
            {
                UserId = organization.Users.FirstOrDefault().Id,
                UserRoleId = 1
            });
            DbContext.SaveChanges();

            return Ok(Mapper.Map<OrganizationDTO>(organization));
        }

        [HttpPut("{id}")]
        [ProducesDefaultResponseType(typeof(OrganizationDTO))]
        public IActionResult Put(int id, [FromBody] Organization updateModel)
        {
            var organization = DbContext.Organizations.FirstOrDefault(x => x.Id == id);
            if (organization == null)
                return NotFound();

            organization.Name = updateModel.Name;
            DbContext.Organizations.Update(organization);
            DbContext.SaveChanges();

            return Ok(Mapper.Map<OrganizationDTO>(organization));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var organization = DbContext.Organizations.FirstOrDefault(x => x.Id == id);
            if (organization == null)
                return NotFound();

            DbContext.Organizations.Remove(organization);
            DbContext.SaveChanges();

            return NoContent();
        }
    }
}
