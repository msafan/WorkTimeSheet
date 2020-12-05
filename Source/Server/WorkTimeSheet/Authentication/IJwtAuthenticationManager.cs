using WorkTimeSheet.DTO;
using WorkTimeSheet.Models;

namespace WorkTimeSheet
{
    public interface IJwtAuthenticationManager
    {
        AuthorizedUser Authenticate(UserDTO user);
    }
}
