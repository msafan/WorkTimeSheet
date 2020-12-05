using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkTimeSheet.DTO
{
    public class OrganizationDTO
    {
        public OrganizationDTO()
        {
            Projects = new HashSet<ProjectDTO>();
            Users = new HashSet<UserDTO>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public virtual ICollection<ProjectDTO> Projects { get; set; }
        public virtual ICollection<UserDTO> Users { get; set; }
    }
}
