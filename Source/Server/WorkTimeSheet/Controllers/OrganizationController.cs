using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.DTO;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Controllers
{
    [Route("api/organization")]
    public class OrganizationController : ApiControllerBase
    {
        public OrganizationController(IDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        //[HttpGet]
        //[ProducesDefaultResponseType(typeof(IEnumerable<OrganizationDTO>))]
        //public IActionResult GetAll()
        //{
        //    var organizations = DbContext.Organizations.Where(x => x.Id == CurrentUser.OrganizationId).Include(x => x.Users).Include(x => x.Projects).ToList();
        //    return Ok(Mapper.Map<IEnumerable<OrganizationDTO>>(organizations));
        //}

        [HttpGet("{id}")]
        [ProducesDefaultResponseType(typeof(OrganizationDTO))]
        public IActionResult GetByUserId(int id)
        {
            var organization = DbContext.Organizations.Include(x => x.Users).Include(x => x.Projects).FirstOrDefault(x => x.Id == CurrentUser.OrganizationId);
            if (organization == null)
                return NotFound();
            return Ok(Mapper.Map<OrganizationDTO>(organization));
        }

        //[HttpPost]
        //[ProducesDefaultResponseType(typeof(OrganizationDTO))]
        //public IActionResult Post([FromBody] Organization organization)
        //{
        //    DbContext.Organizations.Add(organization);
        //    DbContext.SaveChanges();
        //    return Ok(Mapper.Map<OrganizationDTO>(organization));
        //}

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesDefaultResponseType(typeof(OrganizationDTO))]
        public IActionResult Register([FromBody] OrganizationRegistrationModel registrationModel)
        {
            var ownerRole = DbContext.UserRoles.FirstOrDefault(x => x.Role == "Owner");
            var password = PasswordProtector.Create(registrationModel.User.Password);
            var organization = new Organization
            {
                Name = registrationModel.Name,
                Users = new List<User>
                {
                    new User
                    {
                        Email = registrationModel.User.Email,
                        Name = registrationModel.User.Name,
                        Password = password.HashedPassword,
                        Salt = password.Salt,
                        UserRoleMappings = new List<UserRoleMapping>
                        {
                            new UserRoleMapping
                            {
                                UserRoleId=ownerRole.Id
                            }
                        },
                        CurrentWork = new CurrentWork()
                    }
                }
            };

            DbContext.Organizations.Add(organization);
            DbContext.SaveChanges();

            return Ok(Mapper.Map<OrganizationDTO>(organization));
        }

        [HttpPut("{id}")]
        [ProducesDefaultResponseType(typeof(OrganizationDTO))]
        public IActionResult Put(int id, [FromBody] Organization updateModel)
        {
            var organization = DbContext.Organizations.Where(x => x.Id == CurrentUser.OrganizationId).FirstOrDefault(x => x.Id == id);
            if (organization == null)
                return NotFound();

            organization.Name = updateModel.Name;
            organization.Description = updateModel.Description;
            DbContext.Organizations.Update(organization);
            DbContext.SaveChanges();

            return Ok(Mapper.Map<OrganizationDTO>(organization));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var organization = DbContext.Organizations
                .Include(x => x.Users)
                .Include(x => x.Projects)
                .Where(x => x.Id == CurrentUser.OrganizationId).FirstOrDefault(x => x.Id == id);
            if (organization == null)
                return NotFound();

            var users = DbContext.Users.Where(x => x.OrganizationId == organization.Id).ToList();
            if (users != null && users.Any())
                DbContext.Users.RemoveRange(users);

            var projects = DbContext.Projects.Where(x => x.OrganizationId == organization.Id).ToList();
            if (projects != null && projects.Any())
                DbContext.Projects.RemoveRange(projects);

            DbContext.Organizations.Remove(organization);
            DbContext.SaveChanges();

            return NoContent();
        }
    }
}
