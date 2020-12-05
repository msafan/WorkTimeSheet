using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkTimeSheet.Models;

namespace WorkTimeSheet.Authentication
{
    public interface ITokenRefresher
    {
        AuthorizedUser Refresh(AuthorizedUser authorizedUser);
    }
}
