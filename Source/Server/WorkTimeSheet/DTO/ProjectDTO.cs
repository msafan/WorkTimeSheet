using System.Collections.Generic;

namespace WorkTimeSheet.DTO
{
    public class ProjectDTO
    {
        public ProjectDTO()
        {
            WorkLogs = new HashSet<WorkLogDTO>();
        }

        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<WorkLogDTO> WorkLogs { get; set; }
    }
}