using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.DTO;
using WorkTimeSheet.Excepions;

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
            var work = DbContext.CurrentWorks.Include(x => x.User).FirstOrDefault(x => x.User.Id == CurrentUser.Id);
            if (work == null)
                throw new DataNotFoundException($"No work found with user id: {CurrentUser.Id}");

            return Ok(Mapper.Map<CurrentWorkDTO>(work));
        }

        [HttpPatch("start/{projectId}")]
        public IActionResult PatchStartWork(int projectId)
        {
            var work = DbContext.CurrentWorks.FirstOrDefault(x => x.UserId == CurrentUser.Id);
            if (work == null)
                throw new DataNotFoundException($"No work found with user id: {CurrentUser.Id}");

            if (work.ProjectId != null)
                throw new InvalidOperationException($"User: ({CurrentUser.Name}) is already working on some other project");

            var project = DbContext.Projects.FirstOrDefault(x => x.Id == projectId && x.OrganizationId == CurrentUser.OrganizationId);
            if (project == null)
                throw new DataNotFoundException($"No project found with user id: {projectId}");

            var member = DbContext.ProjectMembers.FirstOrDefault(x => x.UserId == CurrentUser.Id && x.ProjectId == project.Id);
            if (member == null)
                throw new DataNotFoundException($"User: ({CurrentUser.Name}) is not associated with project: ({project.Name})");

            work.ProjectId = project.Id;
            work.StartDateTime = DateTime.UtcNow;

            DbContext.CurrentWorks.Update(work);
            DbContext.SaveChanges();

            return NoContent();
        }

        [HttpPatch("stop/{remarks}")]
        public IActionResult PatchStopWork(string remarks)
        {
            var work = DbContext.CurrentWorks.FirstOrDefault(x => x.UserId == CurrentUser.Id);
            if (work == null)
                throw new DataNotFoundException($"No work found with user id: {CurrentUser.Id}");

            if (work.ProjectId == null)
                throw new InvalidOperationException($"User: ({CurrentUser.Name}) is not working on any projects");

            var project = DbContext.Projects.FirstOrDefault(x => x.Id == work.ProjectId && x.OrganizationId == CurrentUser.OrganizationId);
            if (project == null)
                throw new DataNotFoundException($"No project found with project id: {work.ProjectId}");

            var member = DbContext.ProjectMembers.FirstOrDefault(x => x.UserId == CurrentUser.Id && x.ProjectId == project.Id);
            if (member == null)
                throw new DataNotFoundException($"User: ({CurrentUser.Name}) is not associated with project: ({project.Name})");

            DbContext.WorkLogs.Add(new WorkLog
            {
                ProjectId = project.Id,
                UserId = CurrentUser.Id,
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
