using System.Collections.Generic;

namespace WorkTimeSheet.Models
{
    public class CreateUserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<int> RoleIds { get; set; }
    }
}
