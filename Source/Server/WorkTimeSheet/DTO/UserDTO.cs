using System.Collections.Generic;

namespace WorkTimeSheet.DTO
{
    public class UserDTO
    {
        public UserDTO()
        {
            WorkLogs = new HashSet<WorkLogDTO>();
        }

        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public virtual ICollection<WorkLogDTO> WorkLogs { get; set; }
    }
}