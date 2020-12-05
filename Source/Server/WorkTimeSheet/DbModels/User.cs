using System;
using System.Collections.Generic;

#nullable disable

namespace WorkTimeSheet.DbModels
{
    public partial class User
    {
        public User()
        {
            ProjectMembers = new HashSet<ProjectMember>();
            UserRoleMappings = new HashSet<UserRoleMapping>();
            WorkLogs = new HashSet<WorkLog>();
        }

        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual Organization Organization { get; set; }
        public virtual CurrentWork CurrentWork { get; set; }
        public virtual ICollection<ProjectMember> ProjectMembers { get; set; }
        public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; }
        public virtual ICollection<WorkLog> WorkLogs { get; set; }
    }
}
