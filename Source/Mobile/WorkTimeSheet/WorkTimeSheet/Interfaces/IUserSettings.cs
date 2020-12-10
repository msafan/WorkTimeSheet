using System;
using System.Collections.Generic;
using System.Text;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Interfaces
{
    public interface IUserSettings : IUserSettings
    {
        AuthorizedUser AuthorizedUser { get; set; }
    }
}
