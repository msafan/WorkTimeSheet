using System.Collections.Generic;
using WorkTimeSheet.DTO;

namespace WorkTimeSheet.Models
{
    public class AuthorizedUser
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public List<UserRoleDTO> Roles { get; set; }
    }
}
