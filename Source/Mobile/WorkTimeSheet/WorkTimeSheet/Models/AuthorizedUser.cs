using System;
using System.Collections.Generic;
using System.Text;

namespace WorkTimeSheet.Models
{
    public class AuthorizedUser
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public List<UserRole> Roles { get; set; }
    }
}
