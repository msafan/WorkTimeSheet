using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.DTO;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Controllers
{
    [Route("api/project")]
    public class ProjectController : ApiControllerBase
    {
        public ProjectController(IDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] Pagination pagination, [FromQuery] ProjectFilterModel filterModel)
        {
            pagination = pagination?.IsValid() ?? false ? pagination : Pagination.Default;
            filterModel ??= new ProjectFilterModel();

            var query = DbContext.Projects
                .Include(x => x.ProjectMembers)
                .Where(x => x.OrganizationId == CurrentUser.OrganizationId);

            if (!string.IsNullOrEmpty(filterModel.Name))
                query = query.Where(x => x.Name.Contains(filterModel.Name));
            if (!string.IsNullOrEmpty(filterModel.Description))
                query = query.Where(x => x.Description.Contains(filterModel.Description));
            if (filterModel.UserId != null)
                query = query.Where(x => x.ProjectMembers.Any(y => y.UserId == filterModel.UserId));

            query = Paginate(query, pagination, out var paginationToReturn);

            return Ok(new PaginatedResults<ProjectDTO>
            {
                Pagination = paginationToReturn,
                Items = query.Select(x => Mapper.Map<ProjectDTO>(x)).ToList()
            });
        }

        [HttpGet("members/{id}")]
        public IActionResult GetAllMembers(int id, [FromQuery] Pagination pagination, [FromQuery] ProjectMembersFilterModel filterModel)
        {
            pagination = pagination?.IsValid() ?? false ? pagination : Pagination.Default;
            filterModel ??= new ProjectMembersFilterModel();

            var query = DbContext.ProjectMembers
                .Where(x => x.ProjectId == id)
                .Include(x => x.User)
                .ThenInclude(x => x.UserRoleMappings)
                .ThenInclude(x => x.UserRole)
                .Where(x => x.User.OrganizationId == CurrentUser.OrganizationId);

            if (!string.IsNullOrEmpty(filterModel.Name))
                query = query.Where(x => x.User.Name.Contains(filterModel.Name));
            if (!string.IsNullOrEmpty(filterModel.Email))
                query = query.Where(x => x.User.Email.Contains(filterModel.Email));
            if (filterModel.Roles != null && filterModel.Roles.Any())
                query = query.Where(x => x.User.UserRoleMappings.Any(x => filterModel.Roles.Contains(x.UserRole.Role)));

            query = Paginate(query, pagination, out var paginationToReturn);

            return Ok(new PaginatedResults<UserDTO>
            {
                Pagination = paginationToReturn,
                Items = query.Select(x => Mapper.Map<UserDTO>(x.User)).ToList()
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var project = DbContext.Projects.Where(x => x.OrganizationId == CurrentUser.OrganizationId)
                .FirstOrDefault(x => x.Id == id);
            if (project == null)
                return NotFound();

            return Ok(Mapper.Map<ProjectDTO>(project));
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProjectDTO projectDTO)
        {
            var project = new Project
            {
                Name = projectDTO.Name,
                Description = projectDTO.Description,
                OrganizationId = CurrentUser.OrganizationId
            };

            DbContext.Projects.Add(project);
            DbContext.SaveChanges();

            project = DbContext.Projects
                .FirstOrDefault(x => x.Id == project.Id);

            return Ok(Mapper.Map<ProjectDTO>(project));
        }

        [HttpPut("addmembers/{id}")]
        public IActionResult AddMembers(int id, [FromBody] List<int> userIds)
        {
            var project = DbContext.Projects.Where(x => x.OrganizationId == CurrentUser.OrganizationId)
                .FirstOrDefault(x => x.Id == id);

            if (project == null)
                return NotFound();

            var existingusers = DbContext.ProjectMembers.Where(x => x.ProjectId == id).Select(x => x.UserId).ToList();
            var newUsers = userIds.Except(existingusers);

            DbContext.ProjectMembers.AddRange(newUsers.Select(x => new ProjectMember { ProjectId = id, UserId = x }));
            DbContext.SaveChanges();

            return NoContent();
        }

        [HttpPut("removemembers/{id}")]
        public IActionResult RemoveMembers(int id, [FromBody] List<int> userIds)
        {
            var project = DbContext.Projects.Where(x => x.OrganizationId == CurrentUser.OrganizationId)
                .FirstOrDefault(x => x.Id == id);

            if (project == null)
                return NotFound();

            var projectMembers = DbContext.ProjectMembers.Where(x => x.ProjectId == id).Where(x => userIds.Contains(x.UserId)).ToList();

            DbContext.ProjectMembers.RemoveRange(projectMembers);
            DbContext.SaveChanges();

            return NoContent();
        }

        [HttpPut("updatemembers/{id}")]
        public IActionResult UpdateMembers(int id, [FromBody] List<int> userIds)
        {
            var project = DbContext.Projects.Where(x => x.OrganizationId == CurrentUser.OrganizationId)
               .FirstOrDefault(x => x.Id == id);

            if (project == null)
                return NotFound();

            var existingusers = DbContext.ProjectMembers.Where(x => x.ProjectId == id).Select(x => x.UserId).ToList();
            var newUsers = userIds.Except(existingusers);
            var usersToRemove = existingusers.Except(userIds);

            DbContext.ProjectMembers.AddRange(newUsers.Select(x => new ProjectMember { ProjectId = id, UserId = x }));
            
            var projectMembersToRemove = DbContext.ProjectMembers.Where(x => usersToRemove.Contains(x.UserId) && x.ProjectId == id);
            DbContext.ProjectMembers.RemoveRange(projectMembersToRemove);

            DbContext.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ProjectDTO projectDTO)
        {
            var project = DbContext.Projects.Where(x => x.OrganizationId == CurrentUser.OrganizationId)
                .FirstOrDefault(x => x.Id == id);

            if (project == null)
                return NotFound();

            project.Name = projectDTO.Name;
            project.Description = projectDTO.Description;

            DbContext.Projects.Update(project);
            DbContext.SaveChanges();

            project = DbContext.Projects
                .FirstOrDefault(x => x.Id == id);

            return Ok(Mapper.Map<ProjectDTO>(project));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var project = DbContext.Projects
                .Include(x => x.CurrentWorks)
                .Include(x => x.ProjectMembers)
                .Include(x => x.WorkLogs)
                .Where(x => x.OrganizationId == CurrentUser.OrganizationId)
                .FirstOrDefault(x => x.Id == id);
            if (project == null)
                return NotFound();

            //var currentWorks = DbContext.CurrentWorks.Where(x => x.ProjectId == project.Id).ToList();
            //if (currentWorks != null && currentWorks.Any())
            //    DbContext.CurrentWorks.RemoveRange(currentWorks);

            var projectMembers = DbContext.ProjectMembers.Where(x => x.ProjectId == project.Id).ToList();
            if (projectMembers != null && projectMembers.Any())
                DbContext.ProjectMembers.RemoveRange(projectMembers);

            var workLogs = DbContext.WorkLogs.Where(x => x.ProjectId == project.Id).ToList();
            if (workLogs != null && workLogs.Any())
                DbContext.WorkLogs.RemoveRange(workLogs);

            DbContext.Projects.Remove(project);
            DbContext.SaveChanges();

            return NoContent();
        }
    }
}
