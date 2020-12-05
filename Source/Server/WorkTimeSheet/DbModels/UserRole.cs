using System;
using System.Collections.Generic;

#nullable disable

namespace WorkTimeSheet.DbModels
{
    public partial class UserRole
    {
        public UserRole()
        {
            UserRoleMappings = new HashSet<UserRoleMapping>();
        }

        public int Id { get; set; }
        public string Role { get; set; }

        public virtual ICollection<UserRoleMapping> UserRoleMappings { get; set; }
    }
}
