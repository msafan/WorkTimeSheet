using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.DTO;
using WorkTimeSheet.Exceptions;

namespace WorkTimeSheet.Controllers
{
    [Route("api/work")]
    public class WorkController : ApiControllerBase
    {
        public WorkController(IDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        [HttpGet]
        public IActionResult Get()
        {
            var work = DbContext.CurrentWorks.Include(x => x.User).FirstOrDefault(x => x.User.Id == CurrentUserId);
            if (work == null)
                throw new DataNotFoundException($"No work found with user id: {CurrentUserId}");

            return Ok(Mapper.Map<CurrentWorkDTO>(work));
        }

        [HttpPatch("start/{projectId}")]
        public IActionResult PatchStartWork(int projectId)
        {
            var work = DbContext.CurrentWorks.FirstOrDefault(x => x.UserId == CurrentUserId);
            if (work == null)
                throw new DataNotFoundException($"No work found with user id: {CurrentUserId}");

            if (work.ProjectId != null)
                throw new InvalidOperationException($"User: ({CurrentUserName}) is already working on some other project");

            var project = DbContext.Projects.FirstOrDefault(x => x.Id == projectId && x.OrganizationId == CurrentUserOrganizationId);
            if (project == null)
                throw new DataNotFoundException($"No project found with user id: {projectId}");

            var member = DbContext.ProjectMembers.FirstOrDefault(x => x.UserId == CurrentUserId && x.ProjectId == project.Id);
            if (member == null)
                throw new DataNotFoundException($"User: ({CurrentUserName}) is not associated with project: ({project.Name})");

            work.ProjectId = project.Id;
            work.StartDateTime = DateTime.UtcNow;

            DbContext.CurrentWorks.Update(work);
            DbContext.SaveChanges();

            return NoContent();
        }

        [HttpPatch("stop/{remarks}")]
        public IActionResult PatchStopWork(string remarks)
        {
            var work = DbContext.CurrentWorks.FirstOrDefault(x => x.UserId == CurrentUserId);
            if (work == null)
                throw new DataNotFoundException($"No work found with user id: {CurrentUserId}");

            if (work.ProjectId == null)
                throw new InvalidOperationException($"User: ({CurrentUserName}) is not working on any projects");

            var project = DbContext.Projects.FirstOrDefault(x => x.Id == work.ProjectId && x.OrganizationId == CurrentUserOrganizationId);
            if (project == null)
                throw new DataNotFoundException($"No project found with project id: {work.ProjectId}");

            var member = DbContext.ProjectMembers.FirstOrDefault(x => x.UserId == CurrentUserId && x.ProjectId == project.Id);
            if (member == null)
                throw new DataNotFoundException($"User: ({CurrentUserName}) is not associated with project: ({project.Name})");

            DbContext.WorkLogs.Add(new WorkLog
            {
                ProjectId = project.Id,
                UserId = CurrentUserId,
                Remarks = remarks,
                StartDateTime = work.StartDateTime.Value,
                EndDateTime = DateTime.UtcNow,
                TimeInSeconds = (long)(DateTime.UtcNow - work.StartDateTime.Value).TotalSeconds
            });

            work.ProjectId = null;
            work.StartDateTime = null;

            DbContext.CurrentWorks.Update(work);
            DbContext.SaveChanges();

            return NoContent();
        }
    }
}
