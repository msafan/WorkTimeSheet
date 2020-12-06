using System.Collections.Generic;

namespace WorkTimeSheet.DTO
{
    public class UserDTO
    {
        public UserDTO()
        {
            UserRoles = new HashSet<UserRoleDTO>();
        }

        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<UserRoleDTO> UserRoles { get; set; }
    }
}