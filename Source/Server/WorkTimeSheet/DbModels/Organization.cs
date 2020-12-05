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
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
