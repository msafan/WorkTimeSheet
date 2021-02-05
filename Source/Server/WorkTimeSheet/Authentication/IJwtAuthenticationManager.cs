using System;
using WorkTimeSheet.DTO;
using WorkTimeSheet.Models;

namespace WorkTimeSheet
{
    public interface IJwtAuthenticationManager
    {
        AuthorizedUser Authenticate(UserDTO user, bool isRefreshTokenRequired = true, DateTime? accessTokenExpiryDate = null);
    }
}
