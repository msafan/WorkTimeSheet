using System;
using System.Collections.Generic;

#nullable disable

namespace WorkTimeSheet.DbModels
{
    public partial class Organization
    {
        public Organization()
        {
            Projects = new HashSet<Project>();
            Users = new HashSet<User>();
            AccessTokens = new HashSet<AccessToken>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<AccessToken> AccessTokens { get; set; }
    }
}
