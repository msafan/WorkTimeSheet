using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.DTO;
using WorkTimeSheet.Excepions;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Controllers
{
    [Route("api/user")]
    public class UserController : ApiControllerBase
    {
        public UserController(IDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        [HttpGet]
        [Authorize(Roles = Constants.UserRoleOwner)]
        public IActionResult GetAll([FromQuery] Pagination pagination, [FromQuery] UserFilterModel filterModel)
        {
            pagination = pagination?.IsValid() ?? false ? pagination : Pagination.Default;
            filterModel ??= new UserFilterModel();

            var query = DbContext.Users.Include(x => x.UserRoleMappings)
                .ThenInclude(x => x.UserRole)
                .Where(x => x.OrganizationId == CurrentUserOrganizationId);

            if (!string.IsNullOrEmpty(filterModel.Name))
                query = query.Where(x => x.Name.Contains(filterModel.Name));
            if (!string.IsNullOrEmpty(filterModel.Email))
                query = query.Where(x => x.Email.Contains(filterModel.Email));
            if (filterModel.Roles != null && filterModel.Roles.Any())
                query = query.Where(x => x.UserRoleMappings.Any(y => filterModel.Roles.Contains(y.UserRole.Role)));

            query = Paginate(query, pagination, out var paginationToReturn);

            return Ok(new PaginatedResults<UserDTO>
            {
                Pagination = paginationToReturn,
                Items = query.Select(x => Mapper.Map<UserDTO>(x)).ToList()
            });
        }

        [HttpGet("roles")]
        [Authorize(Roles = Constants.UserRoleOwner)]
        public IActionResult GetAllUserRoles()
        {
            var userRoles = DbContext.UserRoles.ToList();
            return Ok(Mapper.Map<IList<UserRoleDTO>>(userRoles));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = DbContext.Users.Where(x => x.OrganizationId == CurrentUserOrganizationId)
                .Include(x => x.UserRoleMappings)
                .ThenInclude(x => x.UserRole)
                .FirstOrDefault(x => x.Id == id);
            if (user == null)
                throw new DataNotFoundException($"No user found on Id: {id}");

            return Ok(Mapper.Map<UserDTO>(user));
        }

        [HttpPost]
        [Authorize(Roles = Constants.UserRoleOwner)]
        public IActionResult Post([FromBody] CreateUserModel createUserModel)
        {
            var password = PasswordProtector.Create(createUserModel.Password);
            var user = new User
            {
                Name = createUserModel.Name,
                Email = createUserModel.Email,
                Password = password.HashedPassword,
                Salt = password.Salt,
                OrganizationId = CurrentUserOrganizationId,
                UserRoleMappings = createUserModel.RoleIds.Select(x => new UserRoleMapping { UserRoleId = x }).ToList(),
                CurrentWork = new CurrentWork()
            };

            DbContext.Users.Add(user);
            DbContext.SaveChanges();

            user = DbContext.Users.Include(x => x.UserRoleMappings)
                .ThenInclude(x => x.UserRole)
                .FirstOrDefault(x => x.Id == user.Id);

            return Ok(Mapper.Map<UserDTO>(user));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Constants.UserRoleOwner)]
        public IActionResult Put(int id, [FromBody] UserDTO userDTO)
        {
            var user = DbContext.Users.Where(x => x.OrganizationId == CurrentUserOrganizationId)
                .Include(x => x.UserRoleMappings)
                .ThenInclude(x => x.UserRole)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
                throw new DataNotFoundException($"No user found on Id: {id}");

            user.Name = userDTO.Name;
            user.Email = userDTO.Email;
            user.UserRoleMappings = userDTO.UserRoles.Select(x => new UserRoleMapping { UserRoleId = x.Id }).ToList();

            DbContext.UserRoleMappings
                .Where(x => x.UserId == id).ToList()
                .ForEach(x => DbContext.UserRoleMappings.Remove(x));

            DbContext.Users.Update(user);
            DbContext.SaveChanges();

            user = DbContext.Users.Include(x => x.UserRoleMappings)
                .ThenInclude(x => x.UserRole)
                .FirstOrDefault(x => x.Id == id);

            return Ok(Mapper.Map<UserDTO>(user));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Constants.UserRoleOwner)]
        public IActionResult Delete(int id)
        {
            var user = DbContext.Users
                .Include(x => x.CurrentWork)
                .Include(x => x.ProjectMembers)
                .Include(x => x.UserRoleMappings)
                .Include(x => x.WorkLogs)
                .Where(x => x.OrganizationId == CurrentUserOrganizationId)
                .FirstOrDefault(x => x.Id == id);
            if (user == null)
                throw new DataNotFoundException($"No user found on Id: {id}");

            var currentWork = DbContext.CurrentWorks.FirstOrDefault(x => x.UserId == user.Id);
            if (currentWork != null)
                DbContext.CurrentWorks.Remove(currentWork);

            var projectMembers = DbContext.ProjectMembers.Where(x => x.UserId == user.Id).ToList();
            if (projectMembers != null && projectMembers.Any())
                DbContext.ProjectMembers.RemoveRange(projectMembers);

            var userRoleMappings = DbContext.UserRoleMappings.Where(x => x.UserId == user.Id).ToList();
            if (userRoleMappings != null && userRoleMappings.Any())
                DbContext.UserRoleMappings.RemoveRange(userRoleMappings);

            var workLogs = DbContext.WorkLogs.Where(x => x.UserId == user.Id).ToList();
            if (workLogs != null && workLogs.Any())
                DbContext.WorkLogs.RemoveRange(workLogs);

            DbContext.Users.Remove(user);
            DbContext.SaveChanges();

            return NoContent();
        }
    }
}
