using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkTimeSheet.Models
{
    public class ProjectMembersFilterModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
    }
}
