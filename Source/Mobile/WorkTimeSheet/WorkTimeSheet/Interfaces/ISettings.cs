using System;
using System.Collections.Generic;
using System.Text;

namespace WorkTimeSheet.Interfaces
{
    public interface IUserSettings
    {
        Plugin.Settings.Abstractions.ISettings AppSettings { get; }
    }
}
