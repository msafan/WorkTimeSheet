using System;
using System.Collections.Generic;

#nullable disable

namespace WorkTimeSheet.DbModels
{
    public partial class Project
    {
        public Project()
        {
            CurrentWorks = new HashSet<CurrentWork>();
            ProjectMembers = new HashSet<ProjectMember>();
            WorkLogs = new HashSet<WorkLog>();
        }

        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Organization Origanization { get; set; }
        public virtual ICollection<CurrentWork> CurrentWorks { get; set; }
        public virtual ICollection<ProjectMember> ProjectMembers { get; set; }
        public virtual ICollection<WorkLog> WorkLogs { get; set; }
    }
}
