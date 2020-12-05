using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WorkTimeSheet.DbModels;

namespace WorkTimeSheet.Controllers
{
    [Route("api/user")]
    public class UserController : ApiControllerBase
    {
        public UserController(IDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {

        }
    }
}
