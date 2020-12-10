using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkTimeSheet.Models
{
    public class UserFilterModel
    {
        public UserFilterModel()
        {
            Roles = new string[0];
        }

        public string Name { get; set; }

        public string Email { get; set; }

        public string[] Roles { get; set; }
    }
}
