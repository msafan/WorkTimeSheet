using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WorkTimeSheet.DbModels;
using WorkTimeSheet.DTO;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Controllers
{
    [Route("api/worklog")]
    public class WorkLogController : ApiControllerBase
    {
        public WorkLogController(IDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] Pagination pagination, [FromQuery] WorkLogFilterModel filterModel)
        {
            pagination = pagination?.IsValid() ?? false ? pagination : Pagination.Default;
            filterModel ??= new WorkLogFilterModel();

            var query = DbContext.WorkLogs
                .Include(x => x.Project)
                .Include(x => x.User)
                .Where(x => x.User.OrganizationId == CurrentUser.OrganizationId);

            if (!string.IsNullOrEmpty(filterModel.Name))
                query = query.Where(x => x.User.Name.Contains(filterModel.Name));
            if (!string.IsNullOrEmpty(filterModel.ProjectName))
                query = query.Where(x => x.Project.Name.Contains(filterModel.ProjectName));
            if (filterModel.StartDate != null)
                query = query.Where(x => x.StartDateTime >= filterModel.StartDate);
            if (filterModel.EndDate != null)
                query = query.Where(x => x.StartDateTime <= filterModel.EndDate);
            if (filterModel.UserId != null)
                query = query.Where(x => x.UserId == filterModel.UserId);
            if (filterModel.ProjectId != null)
                query = query.Where(x => x.ProjectId == filterModel.ProjectId);

            var totalTimeInSeconds = query.Sum(x => x.TimeInSeconds);
            query = Paginate(query, pagination, out var paginationToReturn);

            return Ok(new WorkLogReport
            {
                TotalTime = totalTimeInSeconds,
                PaginatedResults = new PaginatedResults<WorkLogDTO>
                {
                    Pagination = paginationToReturn,
                    Items = query.Select(x => Mapper.Map<WorkLogDTO>(x)).ToList()
                }
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var workLog = DbContext.WorkLogs
                .Include(x => x.Project)
                .Include(x => x.User)
                .Where(x => x.User.OrganizationId == CurrentUser.OrganizationId)
                .FirstOrDefault(x => x.Id == id);
            if (workLog == null)
                return NotFound();

            return Ok(Mapper.Map<WorkLogDTO>(workLog));
        }

        [HttpPost]
        public IActionResult Post([FromBody] WorkLogDTO workLogDTO)
        {
            var workLog = new WorkLog
            {
                EndDateTime = workLogDTO.EndDateTime,
                ProjectId = workLogDTO.ProjectId,
                Remarks = workLogDTO.Remarks,
                StartDateTime = workLogDTO.StartDateTime,
                UserId = workLogDTO.UserId
            };
            workLog.TimeInSeconds = (int)(workLog.EndDateTime - workLog.StartDateTime).TotalSeconds;

            DbContext.WorkLogs.Add(workLog);
            DbContext.SaveChanges();

            workLog = DbContext.WorkLogs.Include(x => x.Project).Include(x => x.User)
                .FirstOrDefault(x => x.Id == workLog.Id);

            return Ok(Mapper.Map<WorkLogDTO>(workLog));
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] WorkLogDTO workLogDTO)
        {
            var workLog = DbContext.WorkLogs
                .Include(x => x.User)
                .Where(x => x.User.OrganizationId == CurrentUser.OrganizationId)
                .FirstOrDefault(x => x.Id == id);

            if (workLog == null)
                return NotFound();

            workLog.EndDateTime = workLogDTO.EndDateTime;
            workLog.ProjectId = workLogDTO.ProjectId;
            workLog.Remarks = workLogDTO.Remarks;
            workLog.StartDateTime = workLogDTO.StartDateTime;
            workLog.UserId = workLogDTO.UserId;

            DbContext.WorkLogs.Update(workLog);
            DbContext.SaveChanges();

            workLog = DbContext.WorkLogs
                .Include(x => x.Project)
                .Include(x => x.User)
                .FirstOrDefault(x => x.Id == id);

            return Ok(Mapper.Map<WorkLogDTO>(workLog));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var workLog = DbContext.WorkLogs
                .Include(x => x.User)
                .Where(x => x.User.OrganizationId == CurrentUser.OrganizationId)
                .FirstOrDefault(x => x.Id == id);
            if (workLog == null)
                return NotFound();

            DbContext.WorkLogs.Remove(workLog);
            DbContext.SaveChanges();

            return NoContent();
        }
    }
}
